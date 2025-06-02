using FluentValidation;
using InsuranceDetails.Messages.Commands;

namespace InsuranceDetails.Api.DataFiles;

public class NServiceBusDataFileService : IDataFileService
{
    private readonly IValidator<DataFile> _validator;
    private readonly IMessageSession _messageSession;

    public NServiceBusDataFileService(IValidator<DataFile> validator, IMessageSession messageSession)
    {
        _validator = validator;
        _messageSession = messageSession;
    }

    public async Task<bool> ProcessDataFileAsync(DataFile dataFile, int healthInsurerId)
    {
        var validationResult = await _validator.ValidateAsync(dataFile);
        if (!validationResult.IsValid)
        {
            return false;
        }

        foreach (var citizenDto in dataFile.Citizens)
        {
            if (citizenDto.BasicHealthInsurance is not null)
            {
                var command = new UpdateBasicHealthInsuranceCommand
                {
                    Bsn = citizenDto.Bsn,
                    HealthInsuranceId = healthInsurerId,
                    AsFromDate = citizenDto.BasicHealthInsurance.AsFromDate,
                    TillDate = citizenDto.BasicHealthInsurance.TillDate
                };
                await _messageSession.Send(command);
            }

            foreach (var supplementaryInsurance in citizenDto.SupplementaryHealthInsurances)
            {
                var command = new UpdateSupplementaryHealthInsuranceCommand
                {
                    Bsn = citizenDto.Bsn,
                    HealthInsuranceId = healthInsurerId,
                    AsFromDate = supplementaryInsurance.AsFromDate,
                    TillDate = supplementaryInsurance.TillDate,
                    WhatIsCovered = supplementaryInsurance.WhatIsCovered,
                    PercentageCovered = supplementaryInsurance.PercentageCovered,
                    MaxAmount = supplementaryInsurance.MaxAmount
                };
                await _messageSession.Send(command);
            }
        }

        return true;
    }
}
