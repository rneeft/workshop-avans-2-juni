using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceDetails.Api.Database;

public class BasicHealthInsurance
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey(nameof(CitizenId))]
    public int CitizenId { get; set; }
    
    [Required]
    [ForeignKey(nameof(HealthInsurerId))]
    public int HealthInsurerId { get; set; }
    
    public required DateTime AsFromDate { get; set; }
    public required DateTime TillDate { get; set; }
    
    public Citizen Citizen { get; set; }
    public HealthInsurer HealthInsurer { get; set; }
}