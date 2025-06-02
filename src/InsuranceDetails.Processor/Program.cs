using InsuranceDetails.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus.Features;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var insuranceDetailsDbConnectionString = builder.Configuration.GetConnectionString("InsuranceDetailsDb") ?? 
                                         throw new InvalidOperationException("No connection string configured");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(insuranceDetailsDbConnectionString));

var messagesDbConnectionString = builder.Configuration.GetConnectionString("MessagesDb") ??
                                 throw new InvalidOperationException("No connection string configured");

var endpointConfiguration = new EndpointConfiguration("DataFileProcessorEndpoint");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.DisableFeature<AutoSubscribe>();
endpointConfiguration.DisableFeature<Audit>();
endpointConfiguration.SendFailedMessagesTo("Error");

// for demo purposes, we limit concurrency to 1
endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

var transport = endpointConfiguration.UseTransport<SqlServerTransport>()
    .QueuePeekerOptions(delay: TimeSpan.FromSeconds(10), peekBatchSize: 1) // let's slow this down for demo purposes
    .ConnectionString(messagesDbConnectionString)
    .DefaultSchema("dbo")
    .Transactions(TransportTransactionMode.SendsAtomicWithReceive);

var delayedDelivery = transport.NativeDelayedDelivery();
delayedDelivery.TableSuffix("Delayed");

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();