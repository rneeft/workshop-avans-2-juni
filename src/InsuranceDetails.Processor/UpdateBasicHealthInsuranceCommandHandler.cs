using InsuranceDetails.Api.Database;
using InsuranceDetails.Messages.Commands;
using Microsoft.EntityFrameworkCore;

namespace InsuranceDetails.Processor;

public class UpdateBasicHealthInsuranceCommandHandler : IHandleMessages<UpdateBasicHealthInsuranceCommand>
{
    private readonly AppDbContext _dbContext;
    private readonly Random _random = new();

    public UpdateBasicHealthInsuranceCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Handle(UpdateBasicHealthInsuranceCommand message, IMessageHandlerContext context)
    {
        var number = _random.Next(1,11);
        if (number == 1)
        {
            throw new InvalidOperationException("Something goes wrong. It happens.");
        }
        
        await Task.Delay(1_000, context.CancellationToken); // Simulate processing time
        
        var citizen = await FindCitizenAsync(message.Bsn);
        var basic = new BasicHealthInsurance
        {
            AsFromDate = message.AsFromDate,
            TillDate = message.TillDate,
            Citizen = citizen,
            HealthInsurerId = message.HealthInsuranceId
        };
        
        _dbContext.BasicHealthInsurances.Add(basic);
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