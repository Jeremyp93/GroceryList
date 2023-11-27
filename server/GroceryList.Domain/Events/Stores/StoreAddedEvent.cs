namespace GroceryList.Domain.Events.Stores;

public record StoreAddedEvent : IDomainEvent
{
    public Guid StoreId { get; private set; }

    public StoreAddedEvent(Guid storeId)
    {
        StoreId = storeId;
    }
}
