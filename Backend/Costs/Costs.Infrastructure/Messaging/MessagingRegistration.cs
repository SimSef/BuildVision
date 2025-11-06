using Aspire.Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;

namespace Costs.Infrastructure.Messaging;

public static class MessagingRegistration
{
    public static IHostApplicationBuilder AddMessaging(this IHostApplicationBuilder builder)
    {
        builder.AddAzureServiceBusClient(connectionName: "messaging");
        return builder;
    }
}

