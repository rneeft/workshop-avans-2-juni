using System.Security.Claims;

namespace InsuranceDetails.Api.Searching;

public static class SearchEndpoints
{
    public static async Task<IResult> SearchBsn(HttpContext httpContext, string bsn, ISearchService searchService)
    {
        var userIdValue = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var userId = 0;
        if (string.IsNullOrWhiteSpace( userIdValue) && !int.TryParse(userIdValue, out userId))
        {
            return Results.Forbid();
        }
        
        var result = await searchService.SearchBsn(bsn, userId);

        return Results.Ok(result);
    }
}