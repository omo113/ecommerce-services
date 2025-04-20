using CatalogService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CatalogService.Infrastructure.Services;

public class MigrationService(IServiceScopeFactory scopeFactory, ILogger<MigrationService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await ExecuteAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    private async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await RunMigration(stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "");
        }
    }

    private async Task RunMigration(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        await MigrateCatalogDatabase(scope, cancellationToken);
    }

    private async Task MigrateCatalogDatabase(
        IServiceScope scope,
        CancellationToken stoppingToken
    )
    {
        var dbContext =
            scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        await EnsureDatabaseAsync(dbContext, stoppingToken);
        await RunMigrationAsync(dbContext, stoppingToken);
    }
    private async Task EnsureDatabaseAsync(
        CatalogDbContext context,
        CancellationToken cancellationToken
    )
    {
        var dbCreator = context.GetService<IRelationalDatabaseCreator>();

        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }
    private static async Task RunMigrationAsync(
        CatalogDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }
}