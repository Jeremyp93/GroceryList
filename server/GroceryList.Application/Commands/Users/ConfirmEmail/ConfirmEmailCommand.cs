using MediatR;

namespace GroceryList.Application.Commands.Users.ConfirmEmail;
public record ConfirmEmailCommand(string Token, string Email) : IRequest<Result>;
