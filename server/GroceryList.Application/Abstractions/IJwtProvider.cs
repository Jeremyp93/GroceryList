using GroceryList.Domain.Aggregates.Users;

namespace GroceryList.Application.Abstractions;
public interface IJwtProvider
{
    string Generate(User user);
}
