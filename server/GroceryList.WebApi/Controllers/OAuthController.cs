using AutoMapper;
using GroceryList.Application.Commands.LoginTwitch;
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
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IMediator _mediator;

    public OAuthController(IMapper mapper, IHttpContextAccessor contextAccessor, IMediator mediator) : base(mapper)
    {
        _contextAccessor = contextAccessor;
        _mediator = mediator;
    }

    [HttpGet("twitch/login")]
    public async Task<IActionResult> TwitchLogin()
    {
        var result = await _mediator.Send(new AuthorizeTwitchCommand());

        if (!result.IsSuccessful)
        {
            ErrorResponse(result);
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
}
