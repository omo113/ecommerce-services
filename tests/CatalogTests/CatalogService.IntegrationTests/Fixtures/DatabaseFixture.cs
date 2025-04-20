using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;

namespace CatalogService.Integration.Tests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;

    public DatabaseFixture()
    {
        var builder = new PostgreSqlBuilder().WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432));
        _postgreSqlContainer = builder.Build();
    }

    public string ConnectionString => _postgreSqlContainer.GetConnectionString();
    public string ContainerId => $"{_postgreSqlContainer.Id}";

    public Task InitializeAsync()
        => _postgreSqlContainer.StartAsync();

    public Task DisposeAsync()
        => _postgreSqlContainer.DisposeAsync().AsTask();
}
