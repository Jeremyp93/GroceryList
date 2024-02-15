using MediatR;

namespace GroceryList.Application.Commands.Users.ForgotPassword;
public record ForgotPasswordCommand(string Email) : IRequest<Result>;
