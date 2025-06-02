using InsuranceDetails.Api.Database;
using InsuranceDetails.Api.IdentityProvider;
using InsuranceDetails.Api.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString("IdentityDb") ?? 
                       throw new InvalidOperationException("No connection string configured");

DatabaseInitialisation.CreateTheDatabase(connectionString, "InsuranceDetails.Api.IdentityProvider.Database.sql-create.sql");

builder.Services
    .AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString))
    .AddIdentityProviderServices(builder.Configuration);

var app = builder.Build();
app.MapDefaultEndpoints();

app.MapIdentityProviderEndpoints();

app.UseHttpsRedirection();

app.Run();