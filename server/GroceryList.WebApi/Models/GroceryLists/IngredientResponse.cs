namespace GroceryList.WebApi.Models.GroceryLists;

public class IngredientResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Category { get; set; } = string.Empty;
}
