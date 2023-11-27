using GroceryList.Domain.Helpers.Contracts;

namespace GroceryList.Domain.Helpers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime Now => DateTime.Now;
}

