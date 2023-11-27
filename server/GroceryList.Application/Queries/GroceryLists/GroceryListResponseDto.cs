using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Aggregates.Stores;

namespace GroceryList.Application.Queries.GroceryLists;
public class GroceryListResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Store? Store { get; set; }
    public IReadOnlyCollection<Ingredient>? Ingredients { get; set; }
    public DateTime? CreatedAt { get; set; }
}
