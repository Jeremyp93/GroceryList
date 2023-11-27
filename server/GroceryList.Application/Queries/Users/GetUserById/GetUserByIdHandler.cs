using GroceryList.Domain.Aggregates.Users;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Users.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IUserRepository _repository;

    public GetUserByIdHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.id);

        return result is null
            ? Result<User>.Failure(ResultStatusCode.NotFound, $"User with id {request.id} was not found")
            : Result<User>.Success(result);
    }
}
