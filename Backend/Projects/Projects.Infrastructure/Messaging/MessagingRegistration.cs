using Aspire.Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Projects.Infrastructure.Messaging.ServiceBus;

namespace Projects.Infrastructure.Messaging;

public static class MessagingRegistration
{
    public static IHostApplicationBuilder AddMessaging(this IHostApplicationBuilder builder)
    {
        builder.AddAzureServiceBusClient(connectionName: "messaging");
        builder.Services.AddHostedService<OutboxPublisherHostedService>();
        builder.Services.AddHostedService<ProjectsEventsSubscriberHostedService>();
        return builder;
    }
}
