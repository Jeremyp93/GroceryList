namespace GroceryList.Domain.Aggregates.Stores;

public record Section
{
    public string Name { get; private set; }
    public int Priority { get; private set; }

    private Section() { }

    public static Section Create(string name, int priority)
    {
        var section = new Section()
        {
            Name = name,
            Priority = priority
        };

        return section;
    }
}
