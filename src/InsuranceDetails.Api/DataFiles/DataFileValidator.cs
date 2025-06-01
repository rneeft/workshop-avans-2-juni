using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace InsuranceDetails.Api.DataFiles;

public class DataFileValidator : AbstractValidator<DataFile>
{
    public DataFileValidator()
    {
        RuleFor(x => x.Citizens)
            .NotEmpty()
            .WithMessage("At least one citizen is required.")
            .Must(citizens => citizens.All(c => !string.IsNullOrEmpty(c.Bsn)))
            .WithMessage("All citizens must have a BSN.");
        
        RuleForEach(x => x.Citizens).ChildRules(citizen =>
        {
            ValidateBasicHealthInsurances(citizen);
            ValidateSupplementaryHealthInsurances(citizen);
        });
    }
    private static void ValidateSupplementaryHealthInsurances(InlineValidator<CitizenDto> citizen)
    {
        citizen.RuleForEach(c => c.SupplementaryHealthInsurances).ChildRules(supplementary =>
        {
            supplementary.RuleFor(s => s.AsFromDate)
                .LessThanOrEqualTo(s => s.TillDate)
                .WithMessage("Supplementary health insurance 'AsFromDate' must be before or equal to 'TillDate'.");

            supplementary.RuleFor(s => s.TillDate)
                .GreaterThanOrEqualTo(s => s.AsFromDate)
                .WithMessage("Supplementary health insurance 'TillDate' must be after or equal to 'AsFromDate'.");

            supplementary.RuleFor(s => s.WhatIsCovered)
                .NotEmpty()
                .WithMessage("Supplementary health insurance 'WhatIsCovered' is required.");

            supplementary.RuleFor(s => s.PercentageCovered)
                .InclusiveBetween(1, 100)
                .WithMessage("Supplementary health insurance 'PercentageCovered' must be between 1 and 100.");

            supplementary.RuleFor(s => s.MaxAmount)
                .GreaterThan(0)
                .WithMessage("Supplementary health insurance 'MaxAmount' must be greater than 0.");
        });
    }

    private static void ValidateBasicHealthInsurances(InlineValidator<CitizenDto> citizen)
    {
        citizen.RuleFor(c => c.BasicHealthInsurance)
            .ChildRules(basic =>
            {
                basic.RuleFor(x=> x!.AsFromDate)
                    .NotNull()
                    .WithMessage("Basic health insurance 'AsFromDate' is required.");
                
                basic.RuleFor(x => x!.AsFromDate)
                    .LessThanOrEqualTo(x => x!.TillDate)
                    .WithMessage("Basic health insurance 'AsFromDate' must be before or equal to 'TillDate'.");

                basic.RuleFor(x => x!.TillDate)
                    .GreaterThanOrEqualTo(b => b!.AsFromDate)
                    .WithMessage("Basic health insurance 'TillDate' must be after or equal to 'AsFromDate'.");
            });
    }
}