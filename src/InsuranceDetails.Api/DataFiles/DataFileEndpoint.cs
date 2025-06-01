using System.Security.Claims;

namespace InsuranceDetails.Api.DataFiles;

public static class DataFileEndpoint
{
    public static async Task<IResult> UploadAsync(HttpContext httpContext, DataFile dataFile, IDataFileService dataFileService)
    {
        var clientId = httpContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (clientId is null || !int.TryParse(clientId, out var clientIdValue))
        {
            return Results.Unauthorized();
        }
        
        var result = await dataFileService.ProcessDataFileAsync(dataFile, clientIdValue);
        return result
            ? Results.Ok()
            : Results.BadRequest("Invalid data file");
    }
}