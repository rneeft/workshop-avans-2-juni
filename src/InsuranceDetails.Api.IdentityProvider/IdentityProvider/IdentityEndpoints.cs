namespace InsuranceDetails.Api.IdentityProvider;

public static class IdentityEndpoints
{
    public static async Task<IResult> LoginAsync(UserLoginRequest request, IUserService service, IIdentityService identityService)
    {
        var user = await service.GetUserAsync(request.Email, request.Password);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var jwt = identityService.GenerateJwtToken(request.Email, user.Id);

        return Results.Text(jwt);
    }

    public static async Task<IResult> CreateAsync(UserCreateRequest request, IUserService service)
    {
        var created = await service.AddUserAsync(request.Name, request.Email, request.Password);

        return created
            ? Results.Ok()
            : Results.BadRequest();
    }
}
