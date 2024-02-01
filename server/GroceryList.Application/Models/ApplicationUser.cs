using AspNetCore.Identity.MongoDbCore.Models;
using GroceryList.Domain.Aggregates.Users;
using MongoDbGenericRepository.Attributes;

namespace GroceryList.Application.Models;

[CollectionName("User")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<OAuthProvider> OAuthProviders { get; set; } = new List<OAuthProvider>();
}
