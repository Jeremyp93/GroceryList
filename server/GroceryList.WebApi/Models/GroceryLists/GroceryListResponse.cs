using GroceryList.WebApi.Models.Stores;

namespace GroceryList.WebApi.Models.GroceryLists;

public class GroceryListResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public StoreResponse? Store { get; set; }
    public List<IngredientResponse>? Ingredients { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}
