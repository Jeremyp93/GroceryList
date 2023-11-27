namespace GroceryList.Domain.Helpers.Contracts;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    DateTime Now { get; }
}

