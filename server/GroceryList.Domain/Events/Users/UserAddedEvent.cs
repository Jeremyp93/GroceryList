namespace GroceryList.Domain.Events.Users;

public record UserAddedEvent : IDomainEvent
{
    public Guid UserId { get; private set; }

    public UserAddedEvent(Guid userId)
    {
        UserId = userId;
    }
}

