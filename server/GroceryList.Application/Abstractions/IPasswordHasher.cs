using GroceryList.Domain.Aggregates.Users;

namespace GroceryList.Application.Abstractions;
public interface IPasswordHasher
{
    bool Verify(string passwordHash, string password);
    string Hash(string password);
}
