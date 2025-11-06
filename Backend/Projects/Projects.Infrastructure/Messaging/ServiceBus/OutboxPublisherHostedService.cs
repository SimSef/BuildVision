using System.Text;

using Azure.Messaging.ServiceBus;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Projects.Infrastructure.Persistence;

namespace Projects.Infrastructure.Messaging.ServiceBus;

internal sealed class OutboxPublisherHostedService(
    IServiceScopeFactory scopeFactory,
    ServiceBusClient client) : BackgroundService
{
    private readonly Dictionary<string, ServiceBusSender> _senders = new(StringComparer.OrdinalIgnoreCase);

    private ServiceBusSender GetSender(string destination)
    {
        if (_senders.TryGetValue(destination, out var s)) return s;
        var created = client.CreateSender(destination);
        _senders[destination] = created;
        return created;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ProjectsDbContext>();

                var pending = await db.Outbox
                    .Where(x => x.Status == "PENDING")
                    .OrderBy(x => x.CreatedAt)
                    .ToListAsync(stoppingToken);

                if (pending.Count == 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                    continue;
                }

                foreach (var evt in pending)
                {
                    try
                    {
                        var destination = evt.DestinationName;
                        var sender = GetSender(destination);

                        var payload = evt.Payload.RootElement.GetRawText();
                        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(payload))
                        {
                            Subject = evt.CeType,
                            ContentType = evt.CeDatacontenttype,
                            MessageId = evt.CeId
                        };

                        using var activity = OutboxPublisherHostedServiceOtel.StartPublish(
                            destination,
                            message.MessageId!,
                            evt.CeId,
                            evt.CeTraceparent,
                            evt.CeTracestate);

                        await sender.SendMessageAsync(message, stoppingToken);

                        evt.Status = "SENT";
                        evt.SentAt = DateTimeOffset.UtcNow;
                        await db.SaveChangesAsync(stoppingToken);
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }
                    catch
                    {
                        evt.Retries += 1;
                        await scope.ServiceProvider.GetRequiredService<ProjectsDbContext>().SaveChangesAsync(stoppingToken);
                    }
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch
            {
                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }

        foreach (var s in _senders.Values)
        {
            try { await s.DisposeAsync(); } catch { }
        }
    }
}
