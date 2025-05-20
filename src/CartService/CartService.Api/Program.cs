using CartService.Api;
using CartService.Api.Endpoints;
using CartService.Api.Extensions;
using CartService.Application;
using CartService.Infrastructure;
using EcommerceServices.ServiceDefaults;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddErrorHandlingWithRules();
builder.Services.AddApplication();
builder.Services.AddServices();
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
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.AddKafka();

builder.AddServiceDefaults();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/scalar"))
        .ExcludeFromDescription();
}
app.MapOpenApi();
app.MapScalarApiReference();

app.MapCartServiceEndpoints();
app.MapCartServiceV2Endpoints();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

try
{
    app.Run();

}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

namespace CartService.Api
{
    public partial class Program;
}