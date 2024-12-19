using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudyMart.MailDev.Hosting;
using Xunit;
using Xunit.Abstractions;

namespace StudyMart.ApiService.Test;

public class ApiFixture : WebApplicationFactory<Program>, IAsyncLifetime, ITestOutputHelperAccessor
{
    private readonly IHost _app;
    public ITestOutputHelper OutputHelper { get; set; }

    public IResourceBuilder<PostgresDatabaseResource> Postgres { get; private set; }
    private string _postgresConnectionString;

    public IResourceBuilder<KeycloakResource> Keycloak { get; private set; }
    private string _keycloakConnectionString;

    public IResourceBuilder<MailDevResource> MailDev { get; private set; }
    private string _mailDevConnectionString;

    public IResourceBuilder<RedisResource> Redis { get; private set; }
    private string _redisConnectionString;

    public ResourceNotificationService ResourceNotificationService { get; private set; }

    public ApiFixture()
    {
        var options = new DistributedApplicationOptions { AssemblyName = typeof(ApiFixture).Assembly.FullName, DisableDashboard = true };
        var appBuilder = DistributedApplication.CreateBuilder(options);

        Postgres = appBuilder.AddPostgres("postgresql").AddDatabase("postgresqldb");
        Keycloak = appBuilder.AddKeycloak("keycloak");
        MailDev = appBuilder.AddMailDev("maildev");
        Redis = appBuilder.AddRedis("cache");

        _app = appBuilder.Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    { $"ConnectionStrings:{Postgres.Resource.Name}", _postgresConnectionString },
                    { $"ConnectionStrings:{Keycloak.Resource.Name}", _keycloakConnectionString },
                    { $"ConnectionStrings:{MailDev.Resource.Name}", _mailDevConnectionString },
                    { $"ConnectionStrings:{Redis.Resource.Name}", _redisConnectionString },
                }!);
            }).ConfigureLogging(p => p.AddXUnit(this));

        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _app.StartAsync();
        _postgresConnectionString = await Postgres.Resource.ConnectionStringExpression.GetValueAsync(default);
        _keycloakConnectionString = Keycloak.GetEndpoint("http").Url;
        _mailDevConnectionString = MailDev.GetEndpoint("http").Url;
        _redisConnectionString = await Redis.Resource.GetConnectionStringAsync() ?? throw new InvalidOperationException("Redis connection string is null");
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _app.StopAsync();
        if (_app is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            _app.Dispose();
        }
    }
}