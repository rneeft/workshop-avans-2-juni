namespace InsuranceDetails.Api.HealthInsurers;

public static class HealthInsurerEndpoints
{
    public static async Task<IResult> CreateAsync(NewHealthInsurerRequest request, IHealthInsurerService courseService)
    {
        var healthInsurer = request.MapToHealthInsurer();
        await courseService.CreateAsync(healthInsurer);
        return Results.Created();
    }

    public static async Task<IResult> UpdateAsync(Guid id, UpdateHealthInsurerRequest request, IHealthInsurerService courseService)
    {
        var healthInsurer = request.MapToHealthInsurer(id);
        await courseService.UpdateAsync(healthInsurer);
        return Results.NoContent();
    }

    public static async Task<IResult> GetByIdAsync(Guid id, IHealthInsurerService courseService)
    {
        var healthInsurer = await courseService.GetByIdAsync(id);
        return healthInsurer is null
            ? Results.NotFound()
            : Results.Ok(healthInsurer);
    }

    public static async Task<IResult> DeleteAsync(Guid id, IHealthInsurerService courseService)
    {
        var deleted = await courseService.DeleteAsync(id);
        return deleted
            ? Results.Ok()
            : Results.NotFound();
    }

    public static async Task<IResult> GetAllAsync(IHealthInsurerService courseService)
    {
        var allHealthInsurers = await courseService.GetAllAsync();
        return Results.Ok(allHealthInsurers);
    }
}

