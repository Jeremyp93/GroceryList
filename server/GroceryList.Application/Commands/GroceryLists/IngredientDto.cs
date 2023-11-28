namespace GroceryList.Application.Commands.GroceryLists;

public class IngredientDto
{
    public string Name { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Category { get; set; } = string.Empty;
}
