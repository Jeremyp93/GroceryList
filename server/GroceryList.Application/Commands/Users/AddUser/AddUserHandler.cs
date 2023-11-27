using GroceryList.Domain.Aggregates.Users;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.Users.AddUser;

public class AddUserHandler : IRequestHandler<AddUserCommand, Result<User>>
{
    private readonly IUserRepository _repository;

    public AddUserHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<User>> Handle(AddUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var newUser = User.Create(command.FirstName, command.LastName, command.Email, command.Password);

            var result = await _repository.AddAsync(newUser);

            return Result<User>.Success(result);
        }
        catch (BusinessValidationException e)
        {
            return Result<User>.Failure(ResultStatusCode.ValidationError, e.Errors);
        }
    }
}
