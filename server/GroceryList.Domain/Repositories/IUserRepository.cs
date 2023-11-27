using GroceryList.Domain.Aggregates.Users;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    
}
