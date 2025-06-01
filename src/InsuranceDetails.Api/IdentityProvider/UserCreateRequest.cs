namespace InsuranceDetails.Api.IdentityProvider;

public class UserCreateRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}
