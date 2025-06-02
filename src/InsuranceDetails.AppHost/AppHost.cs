var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("sql-server", port: 2015)
    .WithDataVolume("avans-workshop")
    .WithLifetime(ContainerLifetime.Persistent);

var apiDatabase = sqlServer
    .AddDatabase("InsuranceDetailsDb");

var api = builder
    .AddProject<Projects.InsuranceDetails_Api>("api")
    .WithReference(apiDatabase)
    .WaitFor(apiDatabase);

var identityDatabase = sqlServer
    .AddDatabase("IdentityDb");

var idp = builder
    .AddProject<Projects.InsuranceDetails_Api_IdentityProvider>("idp")
    .WithReference(identityDatabase)
    .WaitFor(identityDatabase)
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
