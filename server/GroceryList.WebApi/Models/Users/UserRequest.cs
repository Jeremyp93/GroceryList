using System.ComponentModel.DataAnnotations;

namespace GroceryList.WebApi.Models.Users;

public class UserRequest
{
    [Required]
    public required string FirstName { get; set; }
    [Required]
    public required string LastName { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}
