using AutoMapper;
using GroceryList.Application.Commands.LoginGoogle;
using GroceryList.Application.Commands.LoginTwitch;
using GroceryList.Application.Queries.Users.GetUserFromGoogle;
using GroceryList.Application.Queries.Users.GetUserFromTwitch;
using GroceryList.WebApi.Models.OAuth;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GroceryList.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OAuthController : BaseController
{
    private readonly IMediator _mediator;

    public OAuthController(IMapper mapper, IMediator mediator) : base(mapper)
    {
        _mediator = mediator;
    }

    [HttpGet("twitch/login")]
    public async Task<IActionResult> TwitchLogin()
    {
        var result = await _mediator.Send(new AuthorizeTwitchCommand());

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return Redirect(result.Data);
    }

    [HttpPost("twitch/callback")]
    public async Task<IActionResult> TwitchCallback(OAuthRequest oauthRequest)
    {
        var code = oauthRequest.Code;
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest();
        }

        var result = await _mediator.Send(new GetUserFromTwitchQuery(code!));

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

    [HttpGet("google/login")]
    public async Task<IActionResult> GoogleLogin()
    {
        var result = await _mediator.Send(new AuthorizeGoogleCommand());

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return Redirect(result.Data);
    }

    [HttpPost("google/callback")]
    public async Task<IActionResult> GoogleCallback(OAuthRequest oauthRequest)
    {
        var code = oauthRequest.Code;
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest();
        }

        var result = await _mediator.Send(new GetUserFromGoogleQuery(code!));

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
}
