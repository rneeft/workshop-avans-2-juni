using FluentValidation;
using InsuranceDetails.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace InsuranceDetails.Api.DataFiles;

public interface IDataFileService
{
    Task<bool> ProcessDataFileAsync(DataFile dataFile, int healthInsurerId);
}

