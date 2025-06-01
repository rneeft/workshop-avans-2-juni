namespace InsuranceDetails.Api.Searching;

public class SearchResult
{
    public required SearchResultBasic? BasicInsurance { get; init; }
    public required List<SearchResultSupplementary> SupplementaryInsurances { get; init; }
}

public class SearchResultSupplementary
{
    public required string WhatIsCovered { get; init; }
    public int MaximumCoverage { get; init; }
    public int PercentageCoverage { get; init; }
    public required string HealthInsurerName { get; init; }
}

public class SearchResultBasic
{
    public required string HealthInsurerName { get; init; }
}