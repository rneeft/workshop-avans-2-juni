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
using InsuranceDetails.Api.Logging;
using InsuranceDetails.Api.Searching;
using InsuranceDetails.Messages.Commands;

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

AddNServiceBus(builder);

builder.Services
    .AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString))
    .AddAuthenticationAndAuthorisation(builder.Configuration)
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
    .MapHealthInsurerEndpoints()
    .MapDataFileEndpoints()
    .MapSearchEndpoints();

app.UseHttpsRedirection();

app.Run();


static void AddNServiceBus(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("MessagesDb") ?? 
                           throw new InvalidOperationException("No connection string configured");
    
    DatabaseInitialisation.CreateTheDatabase(connectionString, "InsuranceDetails.Api.Database.messages.sql");

    var endpointConfiguration = new EndpointConfiguration("DataFileUploadEndpoint");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.SendOnly();

    var transport = endpointConfiguration.UseTransport<SqlServerTransport>()
        .ConnectionString(connectionString)
        .DefaultSchema("dbo")
        .Transactions(TransportTransactionMode.SendsAtomicWithReceive);

    var delayedDelivery = transport.NativeDelayedDelivery();
    delayedDelivery.TableSuffix("Delayed");

    var routing = transport.Routing();
    routing.RouteToEndpoint(typeof(UpdateSupplementaryHealthInsuranceCommand), "DataFileProcessorEndpoint");
    routing.RouteToEndpoint(typeof(UpdateBasicHealthInsuranceCommand), "DataFileProcessorEndpoint");

    builder.UseNServiceBus(endpointConfiguration);
}

