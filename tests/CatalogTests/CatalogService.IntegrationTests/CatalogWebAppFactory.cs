using CatalogService.Api;
using CatalogService.Infrastructure.Services;
using CatalogService.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CatalogService.Integration.Tests;

public class CatalogWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly DatabaseFixture _database = new();

    protected override void ConfigureWebHost(IWebHostBuilder host)
    {
        host.UseSetting("Aspire:Npgsql:EntityFrameworkCore:PostgreSQL:ConnectionString", _database?.ConnectionString);

        base.ConfigureWebHost(host);

        host.ConfigureServices(ConfigureServices);

        host.ConfigureLogging(config => config.SetMinimumLevel(LogLevel.Error));
        host.UseEnvironment("test");
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var descriptor = services.First(x => x.ImplementationType != null
            && x.ImplementationType.IsAssignableTo(typeof(IMigrationService)));
        services.Remove(descriptor);
        var hostedDescriptor = services.First(x => x.ImplementationType != null
            && x.ImplementationType == typeof(MigrationHostedService));
        services.Remove(hostedDescriptor);
        services.AddScoped<MigrationService>();
    }

    public async Task InitializeAsync()
    {
        await _database.InitializeAsync();
        await Seed();
    }

    public async Task Seed()
    {
        await using var scope = Services.CreateAsyncScope();
        var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
        await migrationService.RunMigration(CancellationToken.None);
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _database.DisposeAsync();
    }
}
