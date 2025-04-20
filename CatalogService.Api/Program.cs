using CatalogService.Infrastructure;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddPostgres(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.ConfigureHttpJsonOptions(json =>
{
    json.SerializerOptions.PropertyNamingPolicy =
        JsonNamingPolicy.CamelCase;
    json.SerializerOptions.WriteIndented = true;
    json.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/scalar"))
        .ExcludeFromDescription();
}
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
