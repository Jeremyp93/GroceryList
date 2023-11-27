using GroceryList.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GroceryList.Infrastructure.Authentication;
public class ClaimReader : IClaimReader
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimReader(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserIdFromClaim()
    {
        if (_httpContextAccessor.HttpContext is null)
            return Guid.Empty;

        if (_httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity claimsIdentity)
        {
            // Accessing individual claims
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId is null ? Guid.Empty : Guid.Parse(userId);
        }

        return Guid.Empty;
   
    }
}
