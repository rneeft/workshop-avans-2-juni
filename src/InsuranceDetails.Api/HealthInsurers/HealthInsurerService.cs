using InsuranceDetails.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace InsuranceDetails.Api.HealthInsurers;

public interface IHealthInsurerService
{
    Task<HealthInsurer?> CreateAsync(HealthInsurer course);
    
    Task<HealthInsurer?> GetByIdAsync(Guid id);
    
    Task<IEnumerable<HealthInsurer>> GetAllAsync();
    
    Task<HealthInsurer?> UpdateAsync(HealthInsurer course);
    
    Task<bool> DeleteAsync(Guid id);
}

public class HealthInsurerService : IHealthInsurerService
{
    private readonly AppDbContext _dbContext;

    public HealthInsurerService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthInsurer?> CreateAsync(HealthInsurer healthInsurer)
    {
        _dbContext.HealthInsurers.Add(healthInsurer);
        await _dbContext.SaveChangesAsync();

        return healthInsurer;
    }

    public async Task<HealthInsurer?> GetByIdAsync(Guid id)
    {
        return await _dbContext.HealthInsurers.FindAsync(id);
    }

    public async Task<IEnumerable<HealthInsurer>> GetAllAsync()
    {
        return await _dbContext.HealthInsurers.ToListAsync();
    }

    public async Task<HealthInsurer?> UpdateAsync(HealthInsurer healthInsurer)
    {
        var existing = await _dbContext.HealthInsurers.FindAsync(healthInsurer.Id);
        if (existing is null)
        {
            return null;
        }

        existing.Name = healthInsurer.Name;
        existing.UpdatedAt = healthInsurer.UpdatedAt;

        await _dbContext.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var healthInsurer = await _dbContext.HealthInsurers.FindAsync(id);
        if (healthInsurer is null)
        {
            return false;
        }

        _dbContext.HealthInsurers.Remove(healthInsurer);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}