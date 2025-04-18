var builder = DistributedApplication.CreateBuilder(args);
var postsDb = builder
    .AddMongoDB("posts-mongodb")
    .WithDataVolume()
    .WithMongoExpress(c => c.WithHostPort(8081))
    .AddDatabase("posts-db");
var api = builder
    .AddProject<Projects.CartService_Api>("api")
    .WithReference(postsDb);
builder.Build().Run();
