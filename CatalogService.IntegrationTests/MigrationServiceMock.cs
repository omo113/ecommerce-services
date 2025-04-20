using CatalogService.Infrastructure.Services;

namespace CatalogService.Integration.Tests;

public class MigrationServiceMock : IMigrationService
{
    public Task RunMigration(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}