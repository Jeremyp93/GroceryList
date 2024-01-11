using GroceryList.Domain.Events.Items;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Aggregates.Items;
public class Item : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public Guid UserId { get; private set; }

    public static Item Create(Guid id, string name, Guid userId)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new BusinessValidationException("Item is invalid", new() { "Name cannot be empty" });
        }

        var item = new Item()
        {
            Id = id,
            Name = name,
            UserId = userId
        };

        item.AddDomainEvent(new ItemAddedEvent(item.Id));
        return item;
    }
}
