using InsuranceDetails.Api.Database;
using InsuranceDetails.Api.HealthInsurers;
using InsuranceDetails.Api.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using InsuranceDetails.Api.DataFiles;
using FluentValidation;
using InsuranceDetails.Api.IdentityProvider;
using InsuranceDetails.Api.Logging;
using InsuranceDetails.Api.Searching;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("InsuranceDetailsDb") ?? 
                       throw new InvalidOperationException("No connection string configured");

DatabaseCreation.MakeSureDatabaseExist(connectionString);
DatabaseInitialisation.CreateTheDatabase(connectionString, "InsuranceDetails.Api.Database.sql-create.sql");

builder.Services
    .AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString))
    .AddAuthenticationAndAuthorisation(builder.Configuration)
    .AddIdentityProviderServices(builder.Configuration)
    .AddLoggingServices()
    .AddSearchServices()
    .AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
    .AddHealthInsurers()
    .AddDataFileService();

var app = builder.Build();
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/liveness", () => Results.Ok("Liveness check OK"));
app.MapGet("/readiness", () => Results.Ok("Readiness check OK"));

app
    .MapIdentityProviderEndpoints()
    .MapHealthInsurerEndpoints()
    .MapDataFileEndpoints()
    .MapSearchEndpoints();

app.UseHttpsRedirection();

app.Run();
