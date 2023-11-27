using GroceryList.Application.Queries.GroceryLists;
using MediatR;

namespace GroceryList.Application.Commands.GroceryLists.AddGroceryList;

public record AddGroceryListCommand() : IRequest<Result<GroceryListResponseDto>>
{
    public string Name { get; set; } = string.Empty;
    public Guid? StoreId { get; set; } = Guid.Empty;
    public List<IngredientDto>? Ingredients { get; set; }
}
