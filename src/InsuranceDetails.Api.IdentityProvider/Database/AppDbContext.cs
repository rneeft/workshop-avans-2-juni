using Microsoft.EntityFrameworkCore;

namespace InsuranceDetails.Api.Database;

public class AppDbContext  : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");

        base.OnModelCreating(modelBuilder);
    }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}