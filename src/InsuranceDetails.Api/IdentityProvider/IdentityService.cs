using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InsuranceDetails.Api.IdentityProvider;

public interface IIdentityService
{
    string GenerateJwtToken(string email, int userId);
}

public class IdentityService : IIdentityService
{
    private readonly IOptions<JwtSettings> _settings;

    public IdentityService(IOptions<JwtSettings> settings)
    {
        _settings = settings;
    }

    public string GenerateJwtToken(string email, int userId)
    {
        var role = email.EndsWith("@workshop.nl", StringComparison.OrdinalIgnoreCase) ? "admin" : "user";
        
        var claims = new List<Claim>
        {
            new (ClaimTypes.Email, email),
            new (ClaimTypes.Role, role),
            new ( ClaimTypes.NameIdentifier, userId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _settings.Value.Issuer,
            _settings.Value.Audience,
            claims,
            expires: DateTime.UtcNow.Add(_settings.Value.Lifetime),
            signingCredentials: credentials);

        
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}