using GroceryList.Domain.Exceptions;

namespace GroceryList.Domain.Aggregates.GroceryLists;

public record Ingredient
{
    private const int MinAmount = 1;

    public string Name { get; private set; } = string.Empty;
    public int Amount { get; private set; } = 0;
    public Category? Category { get; private set; }
    public bool Selected { get; set; }

    private Ingredient() { }

    public static Ingredient Create(string name, int amount, Category category, bool selected)
    {
        var errors = new List<string>();

        if (amount < MinAmount)
        {
            errors.Add($"Amount is lower than the minimum amount ({MinAmount}) authorized");
        }

        if (errors.Count != 0)
        {
            throw new BusinessValidationException("Ingredient is invalid", errors);
        }

        return new Ingredient()
        {
            Name = name,
            Amount = amount,
            Category = category,
            Selected = selected
        };
    }
}
