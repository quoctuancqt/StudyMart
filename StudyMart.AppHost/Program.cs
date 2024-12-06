using StudyMart.AppHost;
using StudyMart.MailDev.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgresql = builder.AddPostgres("postgresql")
                        // .WithPgWeb()
                        .WithLifetime(ContainerLifetime.Persistent);

var posgresqldb = postgresql.AddDatabase("postgresqldb");

var cache = builder.AddRedis("cache")
                    .WithClearCommand()
                    .WithLifetime(ContainerLifetime.Persistent);

var mailDev = builder.AddMailDev("maildev")
                    .WithLifetime(ContainerLifetime.Persistent);

var keycloak = builder.AddKeycloak("keycloak", 8080)
                        // .WithRealmImport("./realms")
                        .WithLifetime(ContainerLifetime.Persistent);

var apiService = builder.AddProject<Projects.StudyMart_ApiService>("apiservice")
    .WithReference(keycloak)
    .WaitFor(keycloak)
    .WithReference(mailDev)
    .WithReference(posgresqldb).WaitFor(posgresqldb);

builder.AddProject<Projects.StudyMart_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(keycloak)
    .WaitFor(keycloak)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.SubscribeAppHostEvent();
builder.SubscribeResourceEvent(cache.Resource);

apiService.WithEnvironment("SCALAR_SERVER_URL", apiService.GetEndpoint("https"));

builder.Build().Run();
