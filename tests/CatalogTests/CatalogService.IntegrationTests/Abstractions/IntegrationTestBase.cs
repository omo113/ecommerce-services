namespace CatalogService.Integration.Tests.Abstractions;


[Collection("Main")]
public abstract class IntegrationTestBase(CatalogWebAppFactory factory)
    : IClassFixture<CatalogWebAppFactory>, IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();

    protected HttpClient Client => _client;

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _client.Dispose();
        return Task.CompletedTask;
    }
}