using GroceryList.Application.Models;

namespace GroceryList.Application.Abstractions;
public interface ITwitchClient
{
    Task<TwitchUser?> GetUser(string code);
}
