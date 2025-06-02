namespace InsuranceDetails.Messages.Commands;

public class UpdateSupplementaryHealthInsuranceCommand : ICommand
{
    public required string Bsn { get; init; }
    public required int HealthInsuranceId { get; init; }
    public required DateTime AsFromDate { get; init; }
    public required DateTime TillDate { get; init; }
    public required string WhatIsCovered { get; init; }
    public required int PercentageCovered { get; init; }
    public required int MaxAmount { get; init; }
}
