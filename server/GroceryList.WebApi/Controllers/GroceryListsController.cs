using AutoMapper;
using GroceryList.Application.Commands.GroceryLists;
using GroceryList.Application.Commands.GroceryLists.AddGroceryList;
using GroceryList.Application.Commands.GroceryLists.RemoveGroceryList;
using GroceryList.Application.Commands.GroceryLists.UpdateGroceryList;
using GroceryList.Application.Commands.GroceryLists.UpdateIngredients;
using GroceryList.Application.Queries.GroceryLists;
using GroceryList.Application.Queries.GroceryLists.GetGroceryListById;
using GroceryList.Application.Queries.GroceryLists.GetGroceryLists;
using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.WebApi.Models.GroceryLists;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryList.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class GroceryListsController : BaseController
{
    private readonly IMediator _mediator;

    public GroceryListsController(IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroceryLists()
    {
        var result = await _mediator.Send(new GetGroceryListsQuery());

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        //return Ok(result.Data);

        return ReturnOk<IEnumerable<GroceryListResponse>, IEnumerable<GroceryListResponseDto>>(result.Data);
    }

    [HttpGet("{id:Guid}", Name = "GetGroceryListById")]
    public async Task<IActionResult> GetGroceryListById([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetGroceryListByIdQuery(id));

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        //return Ok(result.Data);

        return ReturnOk<GroceryListResponse, GroceryListResponseDto>(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> AddGroceryList([FromBody] GroceryListRequest groceryListRequest)
    {
        var result = await _mediator.Send(new AddGroceryListCommand()
        {
            Name = groceryListRequest.Name,
            StoreId = groceryListRequest.StoreId,
            Ingredients = groceryListRequest.Ingredients
        });

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        //return Ok(result.Data);

        return ReturnOk<GroceryListResponse, GroceryListResponseDto>(result.Data);
    }

    [HttpDelete("{id:Guid}", Name = "RemoveGroceryList")]
    public async Task<IActionResult> RemoveGroceryList([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new RemoveGroceryListCommand(id));

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return Ok();

        //return ReturnOk<GroceryListResponse, Domain.Aggregates.GroceryLists.GroceryList>(result.Data);
    }

    [HttpPut("{id:Guid}", Name = "UpdateGroceryList")]
    public async Task<IActionResult> UpdateGroceryList([FromRoute] Guid id, [FromBody] GroceryListRequest groceryListRequest)
    {
        var result = await _mediator.Send(new UpdateGroceryListCommand()
        {
            Id = id,
            Name = groceryListRequest.Name,
            StoreId = groceryListRequest.StoreId,
            Ingredients = groceryListRequest.Ingredients
        });

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        //return Ok(result.Data);

        return ReturnOk<GroceryListResponse, GroceryListResponseDto>(result.Data);

    }

    [HttpPut("{id:Guid}/ingredients", Name = "UpdateIngredients")]
    public async Task<IActionResult> UpdateIngredients([FromRoute] Guid id, [FromBody] List<IngredientDto> ingredients)
    {
        var result = await _mediator.Send(new UpdateIngredientsCommand()
        {
            GroceryListId = id,
            Ingredients = ingredients
        });

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<List<IngredientResponse>, List<Ingredient>>(result.Data);
    }
}
