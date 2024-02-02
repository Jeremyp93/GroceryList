using MediatR;
using System.Text.Json.Serialization;

namespace GroceryList.Domain.SeedWork;

public abstract class AggregateRoot : Entity
{
    public Guid UserId { get; private set; }
    
    private List<INotification> _domainEvents = new List<INotification>();

    [JsonIgnore]
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents = _domainEvents ?? new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        if (_domainEvents is null) return;
        _domainEvents.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    public void AddUser(Guid userId)
    {
        UserId = userId;
    }
}


