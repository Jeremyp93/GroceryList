namespace GroceryList.Domain.Events.GroceryLists;

public record GroceryListAddedEvent : IDomainEvent
{
    public Guid UserId { get; private set; }

    public GroceryListAddedEvent(Guid userId)
    {
        UserId = userId;
    }
}
