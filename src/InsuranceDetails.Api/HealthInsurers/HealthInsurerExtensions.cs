namespace InsuranceDetails.Api.HealthInsurers;

public static class HealthInsurerExtensions
{
    public static IServiceCollection AddHealthInsurers(this IServiceCollection services)
    {
        services.AddScoped<IHealthInsurerService, HealthInsurerService>();

        return services;
    }

    public static WebApplication MapHealthInsurerEndpoints(this WebApplication app)
    {
        app.MapPost("/HealthInsurers", HealthInsurerEndpoints.CreateAsync)
            .RequireAuthorization("Admin");

        app.MapGet("/HealthInsurers/{id:int}", HealthInsurerEndpoints.GetByIdAsync)
            .AllowAnonymous();

        app.MapGet("/HealthInsurers", HealthInsurerEndpoints.GetAllAsync)
            .RequireAuthorization("Admin");

        app.MapPut("/HealthInsurers/{id:int}", HealthInsurerEndpoints.UpdateAsync)
            .RequireAuthorization("Admin");

        app.MapDelete("/HealthInsurers/{id:int}", HealthInsurerEndpoints.DeleteAsync)
            .RequireAuthorization("Admin");

        return app;
    }
}