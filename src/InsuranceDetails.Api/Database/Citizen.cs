using System.ComponentModel.DataAnnotations;

namespace InsuranceDetails.Api.Database;

public class Citizen
{
    [Key]
    public int Id { get; set; }
    public required string Bsn { get; set; }

    public List<BasicHealthInsurance> BasicHealthInsurance { get; set; } = [];
    public List<SupplementaryHealthInsurance> SupplementaryHealthInsurances { get; set; } = [];
}