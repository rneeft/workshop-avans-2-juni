namespace InsuranceDetails.Api.DataFiles;

public class CitizenDto
{
    public required string Bsn { get; set; }
    public BasicHealthInsuranceDto? BasicHealthInsurance { get; set; }
    public List<SupplementaryHealthInsuranceDto> SupplementaryHealthInsurances { get; set; } = [];
}