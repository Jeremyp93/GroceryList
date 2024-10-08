﻿using DnsClient.Internal;
using GroceryList.Application.Abstractions;
using GroceryList.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;

namespace GroceryList.Application.Queries.Users.GetUserFromGoogle;
public class GetUserFromGoogleHandler : IRequestHandler<GetUserFromGoogleQuery, Result<ApplicationUser>>
{
    private const string GoogleKey = "google";
    private readonly IGoogleClient _googleClient;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetUserFromGoogleHandler> _logger;

    public GetUserFromGoogleHandler(IGoogleClient googleClient, UserManager<ApplicationUser> userManager, ILogger<GetUserFromGoogleHandler> logger)
    {
        _googleClient = googleClient;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<ApplicationUser>> Handle(GetUserFromGoogleQuery request, CancellationToken cancellationToken)
    {
        var googleUser = await _googleClient.GetUser(request.Code);
        if (googleUser is null || string.IsNullOrEmpty(googleUser.Email))
        {
            return Result<ApplicationUser>.Failure(ResultStatusCode.ValidationError, "User info could not be retrieved.");
        }

        var existingUser = await _userManager.FindByEmailAsync(googleUser.Email.ToLower());
        if (existingUser is not null && existingUser.OAuthProviders.Any(o => o.OAuthProviderId == googleUser.Id && o.OAuthProviderName == GoogleKey))
        {
            return Result<ApplicationUser>.Success(existingUser);
        }

        var provider = OAuthProvider.Create(googleUser.Id, GoogleKey);
        if (existingUser is not null)
        {
            existingUser.OAuthProviders.Add(provider);
            var updateResult = await _userManager.UpdateAsync(existingUser);
            return updateResult.Succeeded ? Result<ApplicationUser>.Success(existingUser) : Result<ApplicationUser>.Failure(ResultStatusCode.Error, "User could not been logged in.");
        }

        var newUser = new ApplicationUser
        {
            UserName = googleUser.Email,
            Email = googleUser.Email,
            FirstName = googleUser.Name,
        };
        newUser.OAuthProviders.Add(provider);

        var createResult = await _userManager.CreateAsync(newUser);
        if (!createResult.Succeeded)
        {
            _logger.LogError("Create result: {0}", JsonSerializer.Serialize(createResult));
            return Result<ApplicationUser>.Failure(ResultStatusCode.Error, "User could not been created.");
        }
        
        return Result<ApplicationUser>.Success(newUser);
    }
}
