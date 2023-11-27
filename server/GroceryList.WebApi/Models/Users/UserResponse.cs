namespace GroceryList.WebApi.Models.Users;

public class UserResponse
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}
