using AutoMapper;
using GroceryList.Application.Commands.Login;
using GroceryList.Application.Commands.Users.AddUser;
using GroceryList.Application.Queries.Users.GetUserById;
using GroceryList.WebApi.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GroceryList.Application.Abstractions;
using GroceryList.Application.Models;
using GroceryList.Application.Commands.Users.ConfirmEmail;
using System.ComponentModel.DataAnnotations;
using GroceryList.Application.Commands.Users.ForgotPassword;
using GroceryList.Application.Commands.Users.ResetPassword;

namespace GroceryList.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IClaimReader _claimReader;

    public UsersController(IMediator mediator, IMapper mapper, IClaimReader claimReader) : base(mapper)
    {
        _mediator = mediator;
        _claimReader = claimReader;
    }

    [Authorize]
    [HttpGet("{id:Guid}", Name = "GetUserById")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<UserResponse, ApplicationUser>(result.Data);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyself()
    {
        var id = _claimReader.GetUserIdFromClaim();
        var result = await _mediator.Send(new GetUserByIdQuery(id));

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<UserResponse, ApplicationUser>(result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
    {
        var result = await _mediator.Send(new LoginCommand()
        {
            Email = loginRequest.Email,
            Password = loginRequest.Password
        });

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        var claims = new List<Claim>
        {
            new("user_id", result.Data.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return SignIn(new ClaimsPrincipal(identity), new AuthenticationProperties
        {
            IsPersistent = true,
            AllowRefresh = true,
        });
    }

    [HttpPost("logout")]
    public IActionResult LogoutUser()
    {
        return SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
    {
        var result = await _mediator.Send(new AddUserCommand()
        {
            Email = userRequest.Email,
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
            Password = userRequest.Password
        });

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<UserResponse, ApplicationUser>(result.Data);
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
    {
        var result = await _mediator.Send(new ConfirmEmailCommand(token, email));

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return Ok();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([Required][FromQuery] string email)
    {
        var result = await _mediator.Send(new ForgotPasswordCommand(email));

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
    {
        var result = await _mediator.Send(new ResetPasswordCommand
        {
            Email = resetPasswordRequest.Email,
            Password = resetPasswordRequest.Password,
            Token = resetPasswordRequest.Token
        });

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return Ok();
    }
}
