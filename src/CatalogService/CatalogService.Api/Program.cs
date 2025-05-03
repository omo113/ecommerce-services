using CatalogService.Api;
using CatalogService.Api.Extensions;
using CatalogService.Application;
using CatalogService.Infrastructure;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddErrorHandlingWithRules();
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opts.JsonSerializerOptions.WriteIndented = true;
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.AddPostgres();
builder.Services.AddInfrastructure();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();


if (builder.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/scalar"))
        .ExcludeFromDescription();
}
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();


namespace CatalogService.Api
{
    public partial class Program;
}
