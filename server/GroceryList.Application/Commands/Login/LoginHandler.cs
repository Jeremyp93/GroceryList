using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Users;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.Login;
public class LoginHandler : IRequestHandler<LoginCommand, Result<User>>
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginHandler(IUserRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<User>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _repository.SingleOrDefaultAsync(u => u.Email.ToLower() == command.Email.ToLower());
        if (user is null)
        {
            return Result<User>.Failure(ResultStatusCode.ValidationError, "Invalid credentials.");
        }

        if (!_passwordHasher.Verify(user.Password, command.Password))
        {
            return Result<User>.Failure(ResultStatusCode.ValidationError, "Invalid credentials.");
        }

        return Result<User>.Success(user);
    }
}
