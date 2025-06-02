using InsuranceDetails.Api.HealthInsurers;
using InsuranceDetails.Api.Identity;
using Microsoft.EntityFrameworkCore;

namespace InsuranceDetails.Api.Database;

public class AppDbContext  : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HealthInsurer>().ToTable("HealthInsurer");
        modelBuilder.Entity<Citizen>().ToTable("Citizen");
        modelBuilder.Entity<BasicHealthInsurance>().ToTable("BasicHealthInsurance");
        modelBuilder.Entity<SupplementaryHealthInsurance>().ToTable("SupplementaryHealthInsurance");
        modelBuilder.Entity<Log>().ToTable("Log");

        base.OnModelCreating(modelBuilder);
    }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<HealthInsurer> HealthInsurers { get; set; }
    
    public DbSet<Citizen> Citizens { get; set; }
    
    public DbSet<BasicHealthInsurance> BasicHealthInsurances { get; set; }
    
    public DbSet<SupplementaryHealthInsurance> SupplementaryHealthInsurances { get; set; }
    
    public DbSet<Log> Logs {get; set;}
}