using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceDetails.Api.Database;

public class SupplementaryHealthInsurance
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey(nameof(HealthInsurerId))]
    public int HealthInsurerId { get; set; }
    
    [Required]
    [ForeignKey(nameof(CitizenId))]
    public int CitizenId { get; set; }
    
    public required DateTime AsFromDate { get; set; }
    public required DateTime TillDate { get; set; }
    public required string WhatIsCovered { get; set; }
    public required int PercentageCovered { get; set; }
    public required int MaxAmount { get; set; }
    
    public required Citizen Citizen { get; set; }
}