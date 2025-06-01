namespace InsuranceDetails.Api.Identity;

public class JwtSettings
{
    public const string SettingsKey = "Jwt";

    public required string Key { get; init; }
    
    public required TimeSpan Lifetime { get; init; }
    
    public required string Issuer { get; init; }
    
    public required string Audience { get; init; }
}