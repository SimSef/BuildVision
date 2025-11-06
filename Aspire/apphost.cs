#:sdk Aspire.AppHost.Sdk@9.5.2
#:package Aspire.Hosting.AppHost@9.5.2
#:package Aspire.Hosting.Azure.ServiceBus@9.5.2
#:package Aspire.Hosting.Kubernetes@9.5.2-preview.1.25522.3
#:package Aspire.Hosting.PostgreSQL@9.5.2
#:package Aspire.Hosting.NodeJS@9.5.2
#:package Aspire.Hosting.Redis@9.5.2

var builder = DistributedApplication.CreateBuilder(args);

// Backend services (Presentations)
var gateway = builder.AddProject(
    name: "gateway",
    projectPath: "../Backend/Gateway/Gateway.Presentation/Gateway.Presentation.csproj");

var projects = builder.AddProject(
    name: "projects",
    projectPath: "../Backend/Projects/Projects.Presentation/Projects.Presentation.csproj");

var costs = builder.AddProject(
    name: "costs",
    projectPath: "../Backend/Costs/Costs.Presentation/Costs.Presentation.csproj");

// Databases (PostgreSQL)
var projectsPostgres = builder.AddPostgres("projects-postgres")
    .WithHostPort(5433)
    .WithParentRelationship(projects);
var projectsDb = projectsPostgres.AddDatabase("projectsdb");

var costsPostgres = builder.AddPostgres("costs-postgres")
    .WithHostPort(5434)
    .WithParentRelationship(costs);
var costsDb = costsPostgres.AddDatabase("costsdb");

var gatewayPostgres = builder.AddPostgres("gateway-postgres")
    .WithHostPort(5435)
    .WithParentRelationship(gateway);
var gatewayDb = gatewayPostgres.AddDatabase("gatewaydb");

// Messaging (Azure Service Bus)
var serviceBus = builder.AddAzureServiceBus("messaging").RunAsEmulator();

// Topics (one per microservice)
var projectsTopic = serviceBus.AddServiceBusTopic("projects-topic");
var costsTopic = serviceBus.AddServiceBusTopic("costs-topic");
var gatewayTopic = serviceBus.AddServiceBusTopic("gateway-topic");

// Queues (one per microservice)
var projectsQueue = serviceBus.AddServiceBusQueue("projects-queue");
var costsQueue = serviceBus.AddServiceBusQueue("costs-queue");
var gatewayQueue = serviceBus.AddServiceBusQueue("gateway-queue");

// References: each service publishes to its topic and consumes from its queue
projects.WithReference(projectsTopic).WithReference(projectsQueue);
costs.WithReference(costsTopic).WithReference(costsQueue);
gateway.WithReference(gatewayTopic).WithReference(gatewayQueue);

// Subscriptions with auto-forward to per-service queues
// projects consumes from costs and gateway topics
costsTopic.AddServiceBusSubscription("projects-from-costs")
    .WithProperties(s =>
    {
        s.MaxDeliveryCount = 20;
        s.ForwardTo = "projects-queue";
    });
gatewayTopic.AddServiceBusSubscription("projects-from-gateway")
    .WithProperties(s =>
    {
        s.MaxDeliveryCount = 20;
        s.ForwardTo = "projects-queue";
    });

// costs consumes from projects and gateway topics
projectsTopic.AddServiceBusSubscription("costs-from-projects")
    .WithProperties(s =>
    {
        s.MaxDeliveryCount = 20;
        s.ForwardTo = "costs-queue";
    });
gatewayTopic.AddServiceBusSubscription("costs-from-gateway")
    .WithProperties(s =>
    {
        s.MaxDeliveryCount = 20;
        s.ForwardTo = "costs-queue";
    });

// gateway consumes from projects and costs topics
projectsTopic.AddServiceBusSubscription("gateway-from-projects")
    .WithProperties(s =>
    {
        s.MaxDeliveryCount = 20;
        s.ForwardTo = "gateway-queue";
    });
costsTopic.AddServiceBusSubscription("gateway-from-costs")
    .WithProperties(s =>
    {
        s.MaxDeliveryCount = 20;
        s.ForwardTo = "gateway-queue";
    });

// Caches (one per microservice)
var projectsCache = builder.AddRedis("projects-cache").WithHostPort(6381);
var costsCache = builder.AddRedis("costs-cache").WithHostPort(6382);
var gatewayCache = builder.AddRedis("gateway-cache").WithHostPort(6383);

projects.WithReference(projectsCache);
costs.WithReference(costsCache);
gateway.WithReference(gatewayCache);

builder.Build().Run();
