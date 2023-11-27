using GroceryList.Domain.Events.Users;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Aggregates.Users;

public class User : AggregateRoot
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;

    private User()
    {
        /* private constructor only for EF */
    }

    public static User Create(string firstName, string lastName, string email, string password)
    {
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        newUser.AddDomainEvent(new UserAddedEvent(newUser.Id));
        return newUser;
    }
}
