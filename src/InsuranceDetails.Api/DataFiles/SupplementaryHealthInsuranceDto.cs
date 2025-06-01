namespace InsuranceDetails.Api.DataFiles;

public class SupplementaryHealthInsuranceDto
{
    public required DateTime AsFromDate { get; set; }
    public required DateTime TillDate { get; set; }
    public required string WhatIsCovered { get; set; }
    public required int PercentageCovered { get; set; }
    public required int MaxAmount { get; set; }
}