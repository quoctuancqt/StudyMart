using StudyMart.AppHost;
using StudyMart.MailDev.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgresql = builder.AddPostgres("postgresql", port: 5432)
                        .WithPgAdmin()
                        .WithLifetime(ContainerLifetime.Persistent);

var posgresqldb = postgresql.AddDatabase("postgresqldb");

var cache = builder.AddRedis("cache")
                    .WithClearCommand()
                    .WithLifetime(ContainerLifetime.Persistent);

var mailDev = builder.AddMailDev("maildev")
                    .WithLifetime(ContainerLifetime.Persistent);

// https://medium.com/@manuel_gabteni/exporting-keycloak-realms-the-easy-way-using-a-custom-net-aspire-command-aab5b5806b18
var realmName = "keycloak"; 
var keycloakRealmImportPath = "./realms";
var keycloak = builder.AddKeycloak(realmName, 8080)
                        // .WithRealmImport(keycloakRealmImportPath)
                        .WithExportRealmCommand(realmName, keycloakRealmImportPath)
                        .WithLifetime(ContainerLifetime.Persistent);

var apiService = builder.AddProject<Projects.StudyMart_ApiService>("apiservice")
    .WithReference(keycloak)
    .WaitFor(keycloak)
    .WithReference(cache)
    .WaitFor(cache)
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

// var frontend = builder.AddNpmApp("frontend", "../StudyMart.SPA/app", "dev")
//     .WithReference(apiService)
//     .WithEnvironment("BROWSER", "none")
//     .WithHttpEndpoint(env: "VITE_PORT")
//     .WithExternalHttpEndpoints()
//     .PublishAsDockerFile();

builder.SubscribeAppHostEvent();
builder.SubscribeResourceEvent(cache.Resource);

builder.Build().Run();
