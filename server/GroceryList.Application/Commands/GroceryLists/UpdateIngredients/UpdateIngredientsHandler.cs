using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.GroceryLists.UpdateIngredients;
public class UpdateIngredientsHandler : IRequestHandler<UpdateIngredientsCommand, Result<List<Ingredient>>>
{
    private readonly IGroceryListRepository _repository;
    private readonly IClaimReader _claimReader;

    public UpdateIngredientsHandler(IGroceryListRepository repository, IClaimReader claimReader)
    {
        _repository = repository;
        _claimReader = claimReader;
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

            var userId = _claimReader.GetUserIdFromClaim();

            if (list.UserId != userId)
            {
                return Result<List<Ingredient>>.Failure(ResultStatusCode.ValidationError, $"Grocery List does not belong to user {userId}");
            }

            var ingredients = command
              .Ingredients?
              .Select(x => Ingredient.Create(x.Name, x.Amount, Category.Create(x.Category), x.Selected))
              .ToList();

            list.UpdateIngredients(ingredients);

            await _repository.UpdateAsync(list);

            return Result<List<Ingredient>>.Success(ingredients ?? new());
        }
        catch (BusinessValidationException e)
        {
            return Result<List<Ingredient>>.Failure(ResultStatusCode.ValidationError, e.Errors);
        }
    }
}
