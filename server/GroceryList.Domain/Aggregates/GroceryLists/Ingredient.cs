using GroceryList.Domain.Exceptions;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Aggregates.GroceryLists;

public class Ingredient : Entity
{
    private const int MinAmount = 1;

    public string Name { get; private set; } = string.Empty;
    public int Amount { get; private set; } = 0;
    public Category Category { get; private set; }

    private Ingredient() { }

    public static Ingredient Create(string name, int amount, Category category)
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
            Id = Guid.NewGuid(),
            Name = name,
            Amount = amount,
            Category = category
        };
    }
}
