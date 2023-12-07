using AutoMapper;
using GroceryList.Application.Commands.GroceryLists.RemoveGroceryList;
using GroceryList.Application.Commands.GroceryLists.UpdateGroceryList;
using GroceryList.Application.Commands.Stores.AddStores;
using GroceryList.Application.Commands.Stores.RemoveStore;
using GroceryList.Application.Commands.Stores.UpdateStore;
using GroceryList.Application.Queries.GroceryLists;
using GroceryList.Application.Queries.Stores.GetStoreById;
using GroceryList.Application.Queries.Stores.GetStores;
using GroceryList.Domain.Aggregates.Stores;
using GroceryList.WebApi.Models.GroceryLists;
using GroceryList.WebApi.Models.Stores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryList.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StoresController : BaseController
{
    private readonly IMediator _mediator;

    public StoresController(IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetStores()
    {
        var result = await _mediator.Send(new GetStoresQuery());

        if (!result.IsSuccessful)
        {
            ErrorResponse(result);
        }

        return ReturnOk<IEnumerable<StoreResponse>, IEnumerable<Store>>(result.Data);
    }

    [HttpGet("{id:Guid}", Name = "GetStoreById")]
    public async Task<IActionResult> GetStoreById([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetStoreByIdQuery(id));

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<StoreResponse, Store>(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> AddStores([FromBody] StoreRequest storeRequest)
    {
        var result = await _mediator.Send(new AddStoreCommand()
        {
            Name = storeRequest.Name,
            Street = storeRequest.Street,
            ZipCode = storeRequest.ZipCode,
            City = storeRequest.City,
            Country = storeRequest.Country,
            Sections = storeRequest.Sections
        });

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<StoreResponse, Store>(result.Data);
    }

    [HttpDelete("{id:Guid}", Name = "RemoveStore")]
    public async Task<IActionResult> RemoveStore([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new RemoveStoreCommand(id));

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return Ok();
    }

    [HttpPut("{id:Guid}", Name = "UpdateStore")]
    public async Task<IActionResult> UpdateStore([FromRoute] Guid id, [FromBody] StoreRequest storeRequest)
    {
        var result = await _mediator.Send(new UpdateStoreCommand()
        {
            Id = id,
            Name = storeRequest.Name,
            Street = storeRequest.Street,
            ZipCode = storeRequest.ZipCode,
            City = storeRequest.City,
            Country = storeRequest.Country,
            Sections = storeRequest.Sections
        });

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<StoreResponse, Store>(result.Data);
    }
}
