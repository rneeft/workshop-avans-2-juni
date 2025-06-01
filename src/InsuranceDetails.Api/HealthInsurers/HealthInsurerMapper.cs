using InsuranceDetails.Api.Database;

namespace InsuranceDetails.Api.HealthInsurers;

public static class HealthInsurerMapper
{
    public static HealthInsurer MapToHealthInsurer(this NewHealthInsurerRequest request)
    {
        return new HealthInsurer
        {
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ApiKey = Guid.NewGuid().ToString().Replace("-", string.Empty)
        };
    }

    public static HealthInsurer MapToHealthInsurer(this UpdateHealthInsurerRequest request, Guid id)
    {
        return new HealthInsurer
        {
            Name = request.Name,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.MinValue,
            ApiKey = ""
        };
    }
}