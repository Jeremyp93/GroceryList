using AutoMapper;
using GroceryList.Application.Queries.Geocode.Autocomplete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GeocodeAutoCompleteController : BaseController
    {
        private readonly IMediator _mediator;
        public GeocodeAutoCompleteController(IMapper mapper, IMediator mediator) : base(mapper)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> AutoComplete([FromQuery] string searchText)
        {
            var result = await _mediator.Send(new AutoCompleteQuery(searchText));

            if (!result.IsSuccessful)
            {
                ErrorResponse(result);
            }

            return Ok(result.Data);
        }
    }
}
