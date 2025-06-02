using InsuranceDetails.Api.Database;
using InsuranceDetails.Messages.Commands;
using Microsoft.EntityFrameworkCore;

public class UpdateSupplementaryHealthInsuranceCommandHandler : IHandleMessages<UpdateSupplementaryHealthInsuranceCommand>
{
    private readonly AppDbContext _dbContext;
    private readonly Random _random = new();

    public UpdateSupplementaryHealthInsuranceCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Handle(UpdateSupplementaryHealthInsuranceCommand message, IMessageHandlerContext context)
    {
        var number = _random.Next(1,11);
        if (number == 1)
        {
            throw new InvalidOperationException("Something goes wrong. It happens.");
        }
        
        await Task.Delay(1_000, context.CancellationToken); // Simulate processing time
        
        var citizen = await FindCitizenAsync(message.Bsn);
        var newSupplementaryHealthInsurance = new SupplementaryHealthInsurance
        {
            Citizen = citizen,
            HealthInsurerId = message.HealthInsuranceId,
            AsFromDate = message.AsFromDate,
            TillDate = message.TillDate,
            WhatIsCovered = message.WhatIsCovered,
            PercentageCovered = message.PercentageCovered,
            MaxAmount = message.MaxAmount
        };
        
        citizen.SupplementaryHealthInsurances.RemoveAll(x => x.HealthInsurerId == message.HealthInsuranceId && x.WhatIsCovered == message.WhatIsCovered);
        _dbContext.SupplementaryHealthInsurances.Add(newSupplementaryHealthInsurance);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
    
    private async Task<Citizen> FindCitizenAsync(string bsn)
    {
        var citizen = await _dbContext.Citizens
            .FirstOrDefaultAsync(c => c.Bsn == bsn);
        
        if (citizen is null)
        {
            citizen = new Citizen
            {
                Bsn = bsn
            };
                
            _dbContext.Citizens.Add(citizen);
            await _dbContext.SaveChangesAsync();
        }

        return citizen;
    }
}
