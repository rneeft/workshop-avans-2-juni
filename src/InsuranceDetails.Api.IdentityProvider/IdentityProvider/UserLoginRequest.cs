namespace InsuranceDetails.Api.IdentityProvider;

public class UserLoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
