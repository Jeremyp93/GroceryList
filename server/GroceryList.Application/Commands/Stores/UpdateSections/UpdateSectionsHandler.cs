using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Repositories;
using MediatR;
using System.Linq;

namespace GroceryList.Application.Commands.Stores.UpdateSections;
public class UpdateSectionsHandler : IRequestHandler<UpdateSectionsCommand, Result<List<Section>>>
{
    private readonly IStoreRepository _repository;
    private readonly IGroceryListRepository _groceryListRepository;
    private readonly IClaimReader _claimReader;

    public UpdateSectionsHandler(IStoreRepository repository, IClaimReader claimReader, IGroceryListRepository groceryListRepository)
    {
        _repository = repository;
        _claimReader = claimReader;
        _groceryListRepository = groceryListRepository;
    }

    public async Task<Result<List<Section>>> Handle(UpdateSectionsCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var store = await _repository.GetByIdAsync(command.StoreId);
            if (store is null)
            {
                return Result<List<Section>>.Failure(ResultStatusCode.NotFound, $"Store with id {command.StoreId} was not found");
            }

            var userId = _claimReader.GetUserIdFromClaim();

            if (store.UserId != userId)
            {
                return Result<List<Section>>.Failure(ResultStatusCode.ValidationError, $"Store does not belong to user {userId}");
            }

            var sectionsRemoved = command.Sections is null ? store.Sections.Select(s => s.Name).ToList() : store.Sections.Where(s1 => !command.Sections.Any(s2 => s2.Name == s1.Name)).Select(s => s.Name).ToList();

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

            store.UpdateSections(sections);

            await _repository.UpdateAsync(store);

            return Result<List<Section>>.Success(sections ?? new());
        }
        catch (Exception)
        {

            throw;
        }
    }
}
