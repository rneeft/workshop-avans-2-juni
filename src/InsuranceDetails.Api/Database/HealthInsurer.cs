using System.ComponentModel.DataAnnotations;

namespace InsuranceDetails.Api.Database;

public class HealthInsurer
{
    [Key]
    public int Id { get; init; }
    public required string Name { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    
    public required string ApiKey { get; set; }
    
    public List<BasicHealthInsurance> BasicHealthInsurance { get; set; } = [];
    public List<SupplementaryHealthInsurance> SupplementaryHealthInsurances { get; set; } = [];
}