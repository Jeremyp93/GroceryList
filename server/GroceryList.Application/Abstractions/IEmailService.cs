namespace GroceryList.Application.Abstractions;
public interface IEmailService
{
    Task SendTokenEmailAsync(string userEmail, string token);
    Task SendForgotPasswordLink(string userEmail, string token);
}
