namespace InsuranceDetails.Messages.Commands;

public class UpdateBasicHealthInsuranceCommand : ICommand
{
    public required string Bsn { get; init; }
    public required int HealthInsuranceId { get; init; }
    public required DateTime AsFromDate { get; init; }
    public required DateTime TillDate { get; init; }
}