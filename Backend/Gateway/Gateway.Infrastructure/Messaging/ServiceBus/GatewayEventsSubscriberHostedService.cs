using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Hybrid;

namespace Gateway.Infrastructure.Messaging.ServiceBus;

internal sealed class GatewayEventsSubscriberHostedService(ServiceBusClient client, HybridCache cache) : BackgroundService
{
    private ServiceBusProcessor? _processor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor = client.CreateProcessor("gateway-queue", new ServiceBusProcessorOptions
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
        => GatewayEventsSubscriberHostedServiceOtel.OnErrorAsync(args.EntityPath, args.Exception);

    private async Task OnMessageAsync(ProcessMessageEventArgs args)
    {
        var subject = args.Message.Subject;
        var messageId = args.Message.MessageId;
        var body = args.Message.Body.ToString();

        using var activity = GatewayEventsSubscriberHostedServiceOtel.StartConsume(
            queueName: args.EntityPath,
            messageId: messageId,
            subject: subject,
            appProps: args.Message.ApplicationProperties);

        var cacheKey = $"gateway:inbox:{args.EntityPath}:{messageId}";
        _ = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => body,
            tags: ["gateway", "inbox", subject ?? "unknown"]);
    }
}

