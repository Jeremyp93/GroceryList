using GroceryList.Domain.Aggregates.Users;
using GroceryList.Domain.Repositories;

namespace GroceryList.Infrastructure.Repositories.old;

public class UserRepository
{
    private readonly List<User> _users = new()
    {
        User.Create("Jeremy", "Proot", "jeremy.proot@outlook.com", "test123"),
        User.Create("Jean", "Dupuis", "jean.dupuis@outlook.com", "test123")
    };

    public Task<User> AddAsync(User user)
    {
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task DeleteAsync(User entity)
    {
        _users.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_users.AsEnumerable());
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    }

    public Task UpdateAsync(User entity)
    {
        var user = _users.Single(e => e.Id == entity.Id);
        user = entity;
        return Task.CompletedTask;
    }
}
