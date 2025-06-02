using Microsoft.EntityFrameworkCore;

namespace InsuranceDetails.Api.Database;

public class AppDbContext  : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Citizen>().ToTable("Citizen");
        modelBuilder.Entity<BasicHealthInsurance>().ToTable("BasicHealthInsurance");
        modelBuilder.Entity<SupplementaryHealthInsurance>().ToTable("SupplementaryHealthInsurance");

        base.OnModelCreating(modelBuilder);
    }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Citizen> Citizens { get; set; }
    
    public DbSet<BasicHealthInsurance> BasicHealthInsurances { get; set; }
    
    public DbSet<SupplementaryHealthInsurance> SupplementaryHealthInsurances { get; set; }
}