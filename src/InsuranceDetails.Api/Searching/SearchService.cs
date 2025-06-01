using System.Security.Claims;
using InsuranceDetails.Api.Database;
using InsuranceDetails.Api.Logging;
using Microsoft.EntityFrameworkCore;

namespace InsuranceDetails.Api.Searching;

public interface ISearchService
{
    Task<SearchResult> SearchBsn(string bsn, int userId);
}

public class SearchService : ISearchService
{
    private readonly AppDbContext _appDbContext;
    private readonly ILoggingService _loggingService;

    public SearchService(AppDbContext appDbContext, ILoggingService loggingService)
    {
        _appDbContext = appDbContext;
        _loggingService = loggingService;
    }

    public async Task<SearchResult> SearchBsn(string bsn, int userId)
    {
        var basic = await _appDbContext.BasicHealthInsurances
            .Include(x => x.HealthInsurer)
            .Where(x => x.Citizen.Bsn == bsn)
            .FirstOrDefaultAsync(b =>
                DateTime.Now >= b.AsFromDate &&
                DateTime.Now <= b.TillDate);
        
        var supplementaries = await _appDbContext.SupplementaryHealthInsurances
            .Include(x => x.HealthInsurer)
            .Where(x => x.Citizen.Bsn == bsn && DateTime.Now >= x.AsFromDate && DateTime.Now <= x.TillDate)
            .ToListAsync();

        await _loggingService.LogBsnSearch(bsn, userId);

        return new SearchResult
        {
            BasicInsurance = basic is not null  ? new SearchResultBasic  {  HealthInsurerName = basic.HealthInsurer.Name, }  : null,
            SupplementaryInsurances = supplementaries
                .Select(x => new SearchResultSupplementary
                {
                    HealthInsurerName = x.HealthInsurer.Name,
                    WhatIsCovered = x.WhatIsCovered,
                    MaximumCoverage = x.MaxAmount,
                    PercentageCoverage = x.PercentageCovered
                })
                .ToList()
        };
    }
}