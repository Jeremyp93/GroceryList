using AutoMapper;
using GroceryList.Application;
using Microsoft.AspNetCore.Mvc;

namespace GroceryList.WebApi.Controllers;

public class BaseController : ControllerBase
{
    private readonly IMapper _mapper;

    public BaseController(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ActionResult ErrorResponse<T>(Result<T> result)
    {
        return result.StatusCode switch
        {
            ResultStatusCode.ValidationError => BadRequest(result.ErrorMessages),
            ResultStatusCode.NotFound => NotFound(result.ErrorMessages),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessages)
        };
    }

    public ActionResult ReturnOk<T1, T2>(T2 data)
    {
        var response = _mapper.Map<T1>(data);
        return Ok(response);
    }
}
