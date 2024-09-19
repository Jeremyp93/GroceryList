using AutoMapper;
using GroceryList.Application.Queries.Products.GetCategories;
using GroceryList.Application.Queries.Products.GetProducts;
using GroceryList.Domain.Aggregates.Products;
using GroceryList.WebApi.Models.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryList.WebApi.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _mediator.Send(new GetProductsQuery());

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<IEnumerable<ProductResponse>, IEnumerable<Product>>(result.Data);
    }

    [HttpGet]
    [Route("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _mediator.Send(new GetCategoriesQuery());

        if (!result.IsSuccessful)
        {
            return ErrorResponse(result);
        }

        return ReturnOk<IEnumerable<CategoryResponse>, IEnumerable<ProductCategory>>(result.Data);
    }
}
