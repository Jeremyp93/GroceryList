using MediatR;

namespace GroceryList.Application.Commands.LoginGoogle;
public record AuthorizeGoogleCommand : IRequest<Result<string>>;
