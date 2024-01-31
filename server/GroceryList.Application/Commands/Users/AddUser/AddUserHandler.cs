﻿using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Users;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.Users.AddUser;

public class AddUserHandler : IRequestHandler<AddUserCommand, Result<User>>
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public AddUserHandler(IUserRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<User>> Handle(AddUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _repository.SingleOrDefaultAsync(u => u.Email.ToLower() == command.Email.ToLower());
            if (user is not null && !string.IsNullOrEmpty(user.Password))
            {
                return Result<User>.Failure(ResultStatusCode.ValidationError, "Email is already taken");
            }

            var hashedPassword = _passwordHasher.Hash(command.Password);
            User result;
            if (user is not null)
            {
                result = User.Create(user.Id, command.FirstName, command.LastName, user.Email, hashedPassword, user.OAuthProviders.ToList());
                await _repository.UpdateAsync(result);
            }
            else
            {
                var newUser = User.Create(Guid.NewGuid(), command.FirstName, command.LastName, command.Email, hashedPassword);

                result = await _repository.AddAsync(newUser);
            }

            return Result<User>.Success(result);
        }
        catch (BusinessValidationException e)
        {
            return Result<User>.Failure(ResultStatusCode.ValidationError, e.Errors);
        }
    }
}
