using Aspire.Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Costs.Infrastructure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace Costs.Infrastructure.Messaging;

public static class MessagingRegistration
{
    public static IHostApplicationBuilder AddMessaging(this IHostApplicationBuilder builder)
    {
        builder.AddAzureServiceBusClient(connectionName: "messaging");
        builder.Services.AddHostedService<OutboxPublisherHostedService>();
        builder.Services.AddHostedService<CostsEventsSubscriberHostedService>();
        return builder;
    }
}
