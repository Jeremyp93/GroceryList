using MediatR;

namespace GroceryList.Application.Commands.LoginTwitch;
public record AuthorizeTwitchCommand : IRequest<Result<string>>;
