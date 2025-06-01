using InsuranceDetails.Api.Database;

namespace InsuranceDetails.Api.Logging;

public static class LogginServiceExtensions
{
    public static IServiceCollection AddLoggingServices(this IServiceCollection services)
    {
        return services.AddScoped<ILoggingService, LoggingService>();
    }
}

public interface ILoggingService
{
    Task LogBsnSearch(string bsn, int userId);
}

public class LoggingService : ILoggingService
{
    private readonly AppDbContext _appDbContext;

    public LoggingService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task LogBsnSearch(string bsn, int userId)
    {
        var log = new Log
        {
            Action = "BSN Search",
            Bsn = bsn,
            UserId = userId,
            WhenDateTime = DateTime.UtcNow
        };
        
        await _appDbContext.Logs.AddAsync(log);
        await _appDbContext.SaveChangesAsync();
    }
}