using CartService.Api;
using CartService.Api.Endpoints;
using CartService.Api.Extensions;
using CartService.Application;
using CartService.Infrastructure;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddErrorHandlingWithRules();
builder.Services.AddApplication();
builder.AddMongoClient();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(json =>
{
    json.SerializerOptions.PropertyNamingPolicy =
        JsonNamingPolicy.CamelCase;
    json.SerializerOptions.WriteIndented = true;
    json.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/scalar"))
        .ExcludeFromDescription();
}
app.MapOpenApi();
app.MapScalarApiReference();

app.MapCartServiceEndpoints();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace CartService.Api
{
    public partial class Program;
}