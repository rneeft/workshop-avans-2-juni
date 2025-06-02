namespace InsuranceDetails.Api.DataFiles;

public static class DataFileServiceExtensions
{
    public static IServiceCollection AddDataFileService(this IServiceCollection services)
    {
        services.AddScoped<IDataFileService, NServiceBusDataFileService>();

        return services;
    }
    
    public static WebApplication MapDataFileEndpoints(this WebApplication app)
    {
        app.MapPost("/Data", DataFileEndpoint.UploadAsync)
            .RequireAuthorization("HealthInsurer");

        return app;
    }
}