using CartService.Api;
using CartService.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CartService.Integration.Tests;

public class CartWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly DatabaseFixture _database = new();

    protected override void ConfigureWebHost(IWebHostBuilder host)
    {
        host.UseSetting("Aspire:MongoDB:Driver:ConnectionString", _database?.ConnectionString);

        base.ConfigureWebHost(host);

        host.ConfigureServices(ConfigureServices);

        host.ConfigureLogging(config => config.SetMinimumLevel(LogLevel.Error));
        host.UseEnvironment("test");
    }

    private void ConfigureServices(IServiceCollection services)
    {
    }

    public async Task InitializeAsync()
    {
        await _database.InitializeAsync();
        await Seed();
    }

    public async Task Seed()
    {
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _database.DisposeAsync();
    }
}

