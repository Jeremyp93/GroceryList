using GroceryList.Domain.Aggregates.GroceryLists;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList.Application.Commands.GroceryLists.UpdateIngredients;
public record UpdateIngredientsCommand : IRequest<Result<List<Ingredient>>>
{
    public Guid GroceryListId { get; set; } = Guid.Empty;
    public List<IngredientDto>? Ingredients { get; set; }
}
