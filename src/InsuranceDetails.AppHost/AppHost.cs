var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("sql-server", port: 2015)
    .WithDataVolume("avans-workshop")
    .WithLifetime(ContainerLifetime.Persistent);

var apiDatabase = sqlServer
    .AddDatabase("InsuranceDetailsDb");

builder
    .AddProject<Projects.InsuranceDetails_Api>("api")
    .WithReference(apiDatabase)
    .WaitFor(apiDatabase);

builder.Build().Run();
