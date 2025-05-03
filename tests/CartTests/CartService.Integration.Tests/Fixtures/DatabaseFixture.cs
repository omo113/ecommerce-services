using DotNet.Testcontainers.Builders;
using Testcontainers.MongoDb;

namespace CartService.Integration.Tests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer;

    public DatabaseFixture()
    {
        var builder = new MongoDbBuilder().WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017));
        _mongoDbContainer = builder.Build();
    }

    public string ConnectionString => _mongoDbContainer.GetConnectionString();
    public string ContainerId => $"{_mongoDbContainer.Id}";

    public Task InitializeAsync()
        => _mongoDbContainer.StartAsync();

    public Task DisposeAsync()
        => _mongoDbContainer.DisposeAsync().AsTask();
}
