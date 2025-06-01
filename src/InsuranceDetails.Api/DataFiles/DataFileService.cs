using FluentValidation;
using InsuranceDetails.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace InsuranceDetails.Api.DataFiles;

public interface IDataFileService
{
    Task<bool> ProcessDataFileAsync(DataFile dataFile, int healthInsurerId);
}

public class DataFileService : IDataFileService
{
    private readonly Random _random = new();
    private readonly IValidator<DataFile> _validator;
    private readonly AppDbContext _dbContext;

    public DataFileService(IValidator<DataFile> validator, AppDbContext dbContext)
    {
        _validator = validator;
        _dbContext = dbContext;
    }

    public async Task<bool> ProcessDataFileAsync(DataFile dataFile, int healthInsurerId)
    {
        var validationResult = await _validator.ValidateAsync(dataFile);
        if (!validationResult.IsValid)
        {
            return false;
        }

        var healthInsurer = await FindHealthInsurerAsync(healthInsurerId);
        foreach (var citizenDto in dataFile.Citizens)
        {
            await ProcessCitizenAsync(citizenDto, healthInsurer);
        }

        return true;
    }

    private async Task ProcessCitizenAsync(CitizenDto citizenDto, HealthInsurer healthInsurer)
    {
        var number = _random.Next(1,11);
        if (number == 1)
        {
            throw new InvalidOperationException("Something goes wrong. It happens.");
        }
        
        await Task.Delay(number * 100); // Simulate processing time
        var citizen = await FindCitizenAsync(citizenDto.Bsn);
            
        if (citizenDto.BasicHealthInsurance is not null)
        {
            citizen.BasicHealthInsurance.RemoveAll(x => x.HealthInsurerId == healthInsurer.Id);
            
            var basicHealthInsurance = MapToBasicHealthInsurance(citizenDto.BasicHealthInsurance, healthInsurer, citizen);
            citizen.BasicHealthInsurance.Add(basicHealthInsurance);
        }
            
        citizen.SupplementaryHealthInsurances.RemoveAll(x => x.HealthInsurerId == healthInsurer.Id);
        var supplementaryHealthInsurances = citizenDto.SupplementaryHealthInsurances
            .Select(x => MapToSupplementaryHealthInsurance(x, healthInsurer, citizen))
            .ToList();
            
        citizen.SupplementaryHealthInsurances.AddRange(supplementaryHealthInsurances);

        await _dbContext.SaveChangesAsync();
    }

    private static SupplementaryHealthInsurance MapToSupplementaryHealthInsurance(SupplementaryHealthInsuranceDto dto, HealthInsurer healthInsurer, Citizen citizen)
    {
        return new SupplementaryHealthInsurance
        {
            Citizen = citizen,
            HealthInsurer = healthInsurer,
            AsFromDate = dto.AsFromDate,
            TillDate = dto.TillDate,
            WhatIsCovered = dto.WhatIsCovered,
            PercentageCovered = dto.PercentageCovered,
            MaxAmount = dto.MaxAmount
        };
    }
    
    private static BasicHealthInsurance MapToBasicHealthInsurance(BasicHealthInsuranceDto dto, HealthInsurer healthInsurer, Citizen citizen)
    {
        return new BasicHealthInsurance
        {
            Citizen = citizen,
            HealthInsurer = healthInsurer,
            AsFromDate = dto.AsFromDate,
            TillDate = dto.TillDate
        };
    }

    private async Task<Citizen> FindCitizenAsync(string bsn)
    {
        var citizen = await _dbContext.Citizens
            .Include(x => x.BasicHealthInsurance)
            .Include(x => x.SupplementaryHealthInsurances)
            .FirstOrDefaultAsync(c => c.Bsn == bsn);
        
        if (citizen is null)
        {
            citizen = new Citizen
            {
                Bsn = bsn
            };
                
            _dbContext.Citizens.Add(citizen);
        }

        return citizen;
    }
    
    private async Task<HealthInsurer> FindHealthInsurerAsync(int healthInsurerId)
    {
        var healthInsurer = await _dbContext.HealthInsurers
            .FindAsync(healthInsurerId);

        return healthInsurer ?? throw new InvalidOperationException("Unable to find health insurer");
    }
}