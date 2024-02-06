using AutoMapper;
using GroceryList.Application.Commands.Users.ConfirmEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GroceryList.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : BaseController
{
    private readonly IMediator _mediator;

    public EmailController(IMapper mapper, IMediator mediator) : base(mapper)
    {
        _mediator = mediator;
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
}
