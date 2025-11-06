using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Hybrid;

namespace Costs.Infrastructure.Messaging.ServiceBus;

internal sealed class CostsEventsSubscriberHostedService(ServiceBusClient client, HybridCache cache) : BackgroundService
{
    private ServiceBusProcessor? _processor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor = client.CreateProcessor("costs-queue", new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = true,
            MaxConcurrentCalls = 1
        });

        _processor.ProcessMessageAsync += OnMessageAsync;
        _processor.ProcessErrorAsync += OnErrorAsync;

        await _processor.StartProcessingAsync(stoppingToken);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(500, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        finally
        {
            var proc = _processor;
            if (proc is not null)
            {
                try { await proc.StopProcessingAsync(stoppingToken); } catch { }
                try { await proc.DisposeAsync(); } catch { }
            }
        }
    }

    private Task OnErrorAsync(ProcessErrorEventArgs args)
        => CostsEventsSubscriberHostedServiceOtel.OnErrorAsync(args.EntityPath, args.Exception);

    private async Task OnMessageAsync(ProcessMessageEventArgs args)
    {
        var subject = args.Message.Subject;
        var messageId = args.Message.MessageId;
        var body = args.Message.Body.ToString();

        using var activity = CostsEventsSubscriberHostedServiceOtel.StartConsume(
            queueName: args.EntityPath,
            messageId: messageId,
            subject: subject,
            appProps: args.Message.ApplicationProperties);

        var cacheKey = $"costs:inbox:{args.EntityPath}:{messageId}";
        _ = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => body,
            tags: ["costs", "inbox", subject ?? "unknown"]);
    }
}

