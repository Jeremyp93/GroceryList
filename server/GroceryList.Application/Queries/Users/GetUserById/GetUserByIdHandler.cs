using GroceryList.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GroceryList.Application.Queries.Users.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<ApplicationUser>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<ApplicationUser>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _userManager.FindByIdAsync(request.Id.ToString());

        return result is null
            ? Result<ApplicationUser>.Failure(ResultStatusCode.NotFound, $"User with id {request.Id} was not found")
            : Result<ApplicationUser>.Success(result);
    }
}
