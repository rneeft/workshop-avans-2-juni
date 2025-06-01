using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace InsuranceDetails.Api.Identity;

public static class IdentityExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorisation(this IServiceCollection services, ConfigurationManager configuration)
    {
        const string defaultScheme = "DynamicScheme";
        const string apiKeyScheme = "ApiKeyScheme";
        var jwt = configuration.GetSection(JwtSettings.SettingsKey).Get<JwtSettings>()
                  ?? throw new InvalidOperationException("Unable to get JwtSettings from configuration.");
        
        services.AddAuthentication(x =>
            {
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = defaultScheme;
            })
            .AddPolicyScheme(defaultScheme, "Dynamic Authentication", options =>
            {
                options.ForwardDefaultSelector = context => context.Request.Headers.ContainsKey("X-API-KEY") 
                    ? apiKeyScheme 
                    : JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>(apiKeyScheme, options => { });

        
        services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "admin"))
            .AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "admin", "user"))
            .AddPolicy("HealthInsurer", policy => policy.RequireClaim(ClaimTypes.Role, "HealthInsurer"));

        return services
            .Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingsKey));
    }
}