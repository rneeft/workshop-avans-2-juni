namespace InsuranceDetails.Api.IdentityProvider;

public static class IdentityProviderExtensions
{
    public static WebApplication MapIdentityProviderEndpoints(this WebApplication app)
    {
        app.MapPost("/identity/login", IdentityEndpoints.LoginAsync)
            .AllowAnonymous();

        app.MapPost("/identity/create", IdentityEndpoints.CreateAsync)
            .AllowAnonymous();

        return app;
    }

    public static IServiceCollection AddIdentityProviderServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        return services
            .Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingsKey))
            .AddTransient<IUserService, UserService>()
            .AddTransient<IIdentityService, IdentityService>();
    }
}