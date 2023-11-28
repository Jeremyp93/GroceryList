using AutoMapper;
using GroceryList.Application.Commands.Login;
using GroceryList.Application.Commands.Users.AddUser;
using GroceryList.Application.Queries.Users.GetUserById;
using GroceryList.Application.Queries.Users.GetUsers;
using GroceryList.Domain.Aggregates.Users;
using GroceryList.WebApi.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryList.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
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

        return Ok(new TokenResponse { Token = result.Data });
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

        return ReturnOk<UserResponse, User>(result.Data);
    }
}
