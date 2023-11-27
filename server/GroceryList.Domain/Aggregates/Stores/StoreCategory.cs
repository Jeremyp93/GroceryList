using GroceryList.Domain.Aggregates.GroceryLists;

namespace GroceryList.Domain.Aggregates.Stores;

public record StoreCategory
{
    public Category Category { get; private set; }
    public int Priority { get; private set; } = 0;

    private StoreCategory() { }

    public static StoreCategory Create(Category category, int priority)
    {
        return new StoreCategory()
        {
            Category = category,
            Priority = priority
        };
    }
}
