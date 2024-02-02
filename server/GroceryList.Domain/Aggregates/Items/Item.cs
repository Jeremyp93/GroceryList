using GroceryList.Domain.Events.Items;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Aggregates.Items;
public class Item : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;

    public static Item Create(Guid id, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new BusinessValidationException("Item is invalid", new() { "Name cannot be empty" });
        }

        var item = new Item()
        {
            Id = id,
            Name = name
        };

        item.AddDomainEvent(new ItemAddedEvent(item.Id));
        return item;
    }
}
