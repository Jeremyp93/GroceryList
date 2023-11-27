namespace GroceryList.Domain.Aggregates.GroceryLists;

public record Category
{
    public string Name { get; private set; } = string.Empty;

    private Category() { }

    public static Category Create(string name)
    {
        return new Category()
        {
            Name = name,
        };
    }
}
