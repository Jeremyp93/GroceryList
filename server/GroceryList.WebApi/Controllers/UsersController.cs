using AutoMapper;
using GroceryList.Application;
using GroceryList.Application.Commands.Login;
using GroceryList.Application.Commands.Users.AddUser;
using GroceryList.Application.Queries.Users.GetUserById;
using GroceryList.Application.Queries.Users.GetUsers;
using GroceryList.Domain.Aggregates.Users;
using GroceryList.WebApi.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GroceryList.Application.Abstractions;

namespace GroceryList.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    private IClaimReader _claimReader;

    public UsersController(IMediator mediator, IMapper mapper, IClaimReader claimReader) : base(mapper)
    {
        _mediator = mediator;
        _claimReader = claimReader;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _mediator.Send(new GetUsersQuery());

        if (!result.IsSuccessful)
        {
            ErrorResponse(result);
        }

        return ReturnOk<IEnumerable<UserResponse>, IEnumerable<User>>(result.Data);
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

        return ReturnOk<UserResponse, User>(result.Data);
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

        return ReturnOk<UserResponse, User>(result.Data);
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
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
            });

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutUser()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
    {
        var allowedEmails = new string[] { "test@test.ca", "jeremy.proot@outlook.com", "fany.panichelli5@hotmail.com" };
        if (!allowedEmails.Contains(userRequest.Email))
        {
            return ErrorResponse(Result<User>.Failure(ResultStatusCode.ValidationError, "Email is not allowed."));
        }

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

        return ReturnOk<UserResponse, User>(result.Data);
    }
}
