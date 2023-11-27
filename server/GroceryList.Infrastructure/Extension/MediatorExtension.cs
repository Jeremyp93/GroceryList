using GroceryList.Domain.SeedWork;
using MediatR;

namespace GroceryList.Infrastructure.Extension;

public static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, AggregateRoot aggregate)
    {
        if (aggregate.DomainEvents == null || !aggregate.DomainEvents.Any())
        {
            return;
        }

        var domainEvents = aggregate.DomainEvents.ToList();

        aggregate.ClearDomainEvents();

        var tasks = domainEvents
            .Select(async domainEvent =>
            {
                await mediator.Publish(domainEvent);
            });

        await Task.WhenAll(tasks);
    }

    public static async Task DispatchDomainEventsAsync(this IMediator mediator, IEnumerable<AggregateRoot> aggregates)
    {
        var domainEvents = aggregates
            .Where(aggregate => aggregate.DomainEvents != null && aggregate.DomainEvents.Any())
            .SelectMany(x => x.DomainEvents)
            .ToList();

        aggregates.ToList()
        .ForEach(aggregate => aggregate.ClearDomainEvents());

        var tasks = domainEvents
            .Select(async domainEvent =>
            {
                await mediator.Publish(domainEvent);
            });

        await Task.WhenAll(tasks);
    }
}

