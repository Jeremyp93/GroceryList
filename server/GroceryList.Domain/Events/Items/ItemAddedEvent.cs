namespace GroceryList.Domain.Events.Items;
public record ItemAddedEvent : IDomainEvent
{
    public Guid ItemId { get; private set; }

    public ItemAddedEvent(Guid itemId)
    {
        ItemId = itemId;
    }
}
