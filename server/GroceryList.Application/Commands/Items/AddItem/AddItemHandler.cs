using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Items;
using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.Items.AddItem;
internal class AddItemHandler : IRequestHandler<AddItemCommand, Result<Item>>
{
    private readonly IItemRepository _repository;
    private readonly IClaimReader _claimReader;

    public AddItemHandler(IItemRepository repository, IClaimReader claimReader)
    {
        _repository = repository;
        _claimReader = claimReader;
    }

    public async Task<Result<Item>> Handle(AddItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _claimReader.GetUserIdFromClaim();

            var existingItems = await _repository.WhereAsync(l => l.UserId == userId && l.Name == command.Name, null, cancellationToken);
            if (existingItems is not null && existingItems.ToList().Any())
            {
                return Result<Item>.Failure(ResultStatusCode.Error, $"Item with name {command.Name} already exists.");
            }

            var newItem = Item.Create(Guid.NewGuid(), command.Name);

            var result = await _repository.AddAsync(newItem);

            return Result<Item>.Success(result);
        }
        catch (BusinessValidationException e)
        {
            return Result<Item>.Failure(ResultStatusCode.ValidationError, e.Errors);
        }
    }
}
