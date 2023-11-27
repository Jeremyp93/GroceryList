using GroceryList.Domain.Aggregates.Users;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Users.GetUsers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, Result<IEnumerable<User>>>
{
    private readonly IUserRepository _repository;

    public GetUsersHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<User>>.Success(result);
    }

}
