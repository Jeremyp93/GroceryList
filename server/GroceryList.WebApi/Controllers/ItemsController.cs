using AutoMapper;
using GroceryList.Application.Commands.Items.AddItem;
using GroceryList.Application.Commands.Items.RemoveItem;
using GroceryList.Application.Queries.Items.GetItems;
using GroceryList.Domain.Aggregates.Items;
using GroceryList.WebApi.Models.Items;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryList.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ItemsController : BaseController
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        var result = await _mediator.Send(new GetItemsQuery());

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<IEnumerable<ItemResponse>, IEnumerable<Item>>(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] ItemRequest itemRequest)
    {
        var result = await _mediator.Send(new AddItemCommand(itemRequest.Name));

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<ItemResponse, Item>(result.Data);
    }

    [HttpDelete("{id:Guid}", Name = "RemoveItem")]
    public async Task<IActionResult> RemoveItem([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new RemoveItemCommand(id));

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return Ok();
    }
}
