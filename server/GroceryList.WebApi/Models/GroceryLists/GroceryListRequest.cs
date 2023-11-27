using GroceryList.Application.Commands.GroceryLists;

namespace GroceryList.WebApi.Models.GroceryLists;

public class GroceryListRequest
{
    public string Name { get; set; }
    public Guid UserId { get; set; }
    public Guid? StoreId { get; set; }
    public List<IngredientDto>? Ingredients { get; set; }
}
