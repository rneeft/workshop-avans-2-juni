using System.ComponentModel.DataAnnotations;

namespace InsuranceDetails.Api.Database;

public class User
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required byte[] Salt { get; set; }
}
