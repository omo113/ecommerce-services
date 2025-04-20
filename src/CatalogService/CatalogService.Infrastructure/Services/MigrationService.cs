using CatalogService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CatalogService.Infrastructure.Services;

public interface IMigrationService
{
    Task RunMigration(CancellationToken cancellationToken);
}
public class MigrationService(CatalogDbContext dbContext) : IMigrationService
{
    public async Task RunMigration(CancellationToken cancellationToken)
    {
        await MigrateCatalogDatabase(cancellationToken);
    }

    private async Task MigrateCatalogDatabase(
        CancellationToken stoppingToken
    )
    {
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