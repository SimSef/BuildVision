using Aspire.Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Gateway.Infrastructure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Infrastructure.Messaging;

public static class MessagingRegistration
{
    public static IHostApplicationBuilder AddMessaging(this IHostApplicationBuilder builder)
    {
        builder.AddAzureServiceBusClient(connectionName: "messaging");
        builder.Services.AddHostedService<OutboxPublisherHostedService>();
        builder.Services.AddHostedService<GatewayEventsSubscriberHostedService>();
        return builder;
    }
}
