using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Repositories;
using MediatR;
using System.Collections.Generic;

namespace GroceryList.Application.Commands.GroceryLists.UpdateIngredients;
public class UpdateIngredientsHandler : IRequestHandler<UpdateIngredientsCommand, Result<List<Ingredient>>>
{
    private readonly IGroceryListRepository _repository;

    public UpdateIngredientsHandler(IGroceryListRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<Ingredient>>> Handle(UpdateIngredientsCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var list = await _repository.GetByIdAsync(command.GroceryListId);
            if (list is null)
            {
                return Result<List<Ingredient>>.Failure(ResultStatusCode.NotFound, $"Grocery List with id {command.GroceryListId} was not found");
            }

            var ingredients = command
              .Ingredients?
              .Select(x => Ingredient.Create(x.Name, x.Amount, Category.Create(x.Category)))
              .ToList();

            list.UpdateIngredients(ingredients);

            await _repository.UpdateAsync(list);

            return Result<List<Ingredient>>.Success(ingredients ?? new());
        }
        catch (Exception)
        {

            throw;
        }
    }
}
