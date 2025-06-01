namespace InsuranceDetails.Api.Searching;

public static class SearchExtensions
{
    public static IServiceCollection AddSearchServices(this IServiceCollection services)
    {
        services.AddScoped<ISearchService, SearchService>();

        return services;
    }
    
    public static WebApplication MapSearchEndpoints(this WebApplication app)
    {
        app.MapGet("/Search/{bsn}", SearchEndpoints.SearchBsn)
            .RequireAuthorization("User");

        return app;
    }
}