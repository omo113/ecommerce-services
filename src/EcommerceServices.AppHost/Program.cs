var builder = DistributedApplication.CreateBuilder(args);

var pAdmin = builder.AddParameter("postgres-admin");
var admin = builder.AddParameter("admin");
var password = builder.AddParameter("admin-password", secret: true);

var cartsDb = builder
    .AddMongoDB("carts-mongodb")
    .WithDataVolume()
    .WithMongoExpress(c => c.WithHostPort(8081))
    .AddDatabase("carts-db");
var catalogDb = builder
    .AddPostgres("dbserver", pAdmin, password)
    .WithDataVolume()
    .WithPgAdmin(c => c.WithHostPort(5050))
    .AddDatabase("catalog-db");

var cartApi = builder
    .AddProject<Projects.CartService_Api>("api")
    .WithReference(cartsDb)
    .WithReplicas(1);

var catalogApi = builder
    .AddProject<Projects.CatalogService_Api>("api")
    .WithReference(cartsDb)
    .WithReplicas(1);


builder.Build().Run();
