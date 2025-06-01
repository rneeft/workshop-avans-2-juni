using System.Security.Claims;
using System.Text.Encodings.Web;
using InsuranceDetails.Api.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InsuranceDetails.Api.Identity;

public class ApiKeyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly AppDbContext _dbContext;

    public ApiKeyAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        AppDbContext dbContext)
        : base(options, logger, encoder)
    {
        _dbContext = dbContext;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-API-KEY", out var apiKeyValue))
        {
            return AuthenticateResult.NoResult();
        }

        var healthInsurer = await _dbContext.HealthInsurers
            .FirstOrDefaultAsync(c => c.ApiKey == apiKeyValue.ToString());

        if (healthInsurer is null)
        {
            return AuthenticateResult.Fail("Invalid API Key");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, healthInsurer.Name),
            new(ClaimTypes.NameIdentifier, healthInsurer.Id.ToString()),
            new(ClaimTypes.Role, "HealthInsurer")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}