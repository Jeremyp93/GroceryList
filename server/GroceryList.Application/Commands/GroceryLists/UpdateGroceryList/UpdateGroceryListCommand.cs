using GroceryList.Application.Queries.GroceryLists;
using MediatR;

namespace GroceryList.Application.Commands.GroceryLists.UpdateGroceryList;
public record UpdateGroceryListCommand() : IRequest<Result<GroceryListResponseDto>>
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid? StoreId { get; set; } = Guid.Empty;
    public List<IngredientDto>? Ingredients { get; set; }
}
