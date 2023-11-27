using GroceryList.Application.Abstractions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.Login;
public class LoginHandler : IRequestHandler<LoginCommand, Result<string>>
{
    private readonly IUserRepository _repository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;

    public LoginHandler(IUserRepository repository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _repository.SingleOrDefaultAsync(u => u.Email == command.Email, "");
        if (user is null)
        {
            return Result<string>.Failure(ResultStatusCode.ValidationError, "Invalid credentials.");
        }

        if (!_passwordHasher.Verify(user.Password, command.Password))
        {
            return Result<string>.Failure(ResultStatusCode.ValidationError, "Invalid credentials.");
        }

        var token = _jwtProvider.Generate(user);

        return Result<string>.Success(token);
    }
}
