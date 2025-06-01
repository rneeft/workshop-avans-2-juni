using System.ComponentModel.DataAnnotations;

namespace InsuranceDetails.Api.Database;

public class Log
{
    [Key]
    public int Id { get; init; }
    
    [Required]
    [MaxLength(100)]
    public required string Action { get; set; }
    
    [Required]
    [MaxLength(10)]
    public required string Bsn { get; set; }
    
    public int UserId { get; init; }
    
    public DateTime WhenDateTime { get; init; }
}