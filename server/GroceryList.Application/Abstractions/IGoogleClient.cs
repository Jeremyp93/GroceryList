using GroceryList.Application.Models;

namespace GroceryList.Application.Abstractions;
public interface IGoogleClient
{
    Task<GoogleUser?> GetUser(string code);
}
