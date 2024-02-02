using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.Stores.UpdateStore;
public class UpdateStoreHandler : IRequestHandler<UpdateStoreCommand, Result<Store>>
{
    private readonly IStoreRepository _repository;
    private readonly IGroceryListRepository _groceryListRepository;
    private readonly IClaimReader _claimReader;

    public UpdateStoreHandler(IStoreRepository repository, IClaimReader claimReader, IGroceryListRepository groceryListRepository)
    {
        _repository = repository;
        _claimReader = claimReader;
        _groceryListRepository = groceryListRepository;
    }

    public async Task<Result<Store>> Handle(UpdateStoreCommand command, CancellationToken cancellationToken)
    {
        var store = await _repository.GetByIdAsync(command.Id);
        if (store is null)
        {
            return Result<Store>.Failure(ResultStatusCode.NotFound, $"Store with id {command.Id} was not found");
        }

        try
        {
            var userId = _claimReader.GetUserIdFromClaim();

            if (store.UserId != userId)
            {
                return Result<Store>.Failure(ResultStatusCode.ValidationError, $"Store does not belong to user {userId}");
            }

            var sectionsRemoved = store.Sections.Where(s1 => command.Sections != null && !command.Sections.Any(s2 => s2.Name == s1.Name)).Select(s => s.Name).ToList();

            if (sectionsRemoved.Any())
            {
                var listsToUpdate = (await _groceryListRepository.WhereAsync(
                        l => l.StoreId == store.Id && l.Ingredients.Any(i => sectionsRemoved.Contains(i.Category == null ? "" : i.Category.Name)),
                        null,
                        cancellationToken
                    )).ToList();
                foreach (var list in listsToUpdate)
                {
                    var updatedIngredients = list.Ingredients.Select(ingredient =>
                    {
                        if (sectionsRemoved.Contains(ingredient.Category == null ? "" : ingredient.Category.Name))
                        {
                            return Ingredient.Create(ingredient.Name, ingredient.Amount, Category.Create(string.Empty), ingredient.Selected);
                        }
                        return ingredient;
                    }).ToList();

                    list.UpdateIngredients(updatedIngredients);
                }
                await _groceryListRepository.UpdateRangeAsync(listsToUpdate);
            }

            var sections = command
              .Sections?
              .Select(x => Section.Create(x.Name, x.Priority))
              .ToList();

            var address = Address.Create(command.Street, command.City, command.Country, command.ZipCode);

            store.Update(command.Name, sections, address);

            await _repository.UpdateAsync(store);

            return Result<Store>.Success(store);
        }
        catch (BusinessValidationException e)
        {
            return Result<Store>.Failure(ResultStatusCode.ValidationError, e.Errors);
        }
    }
}
