namespace GroceryList.WebApi.Models.Users;

public class ConfirmEmailRequest
{
    public required string Token { get; set; }
    public required string Email { get; set; }
}
