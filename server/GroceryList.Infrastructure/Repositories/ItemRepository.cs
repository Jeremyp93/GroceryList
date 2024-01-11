using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Items;
using GroceryList.Domain.Helpers.Contracts;
using GroceryList.Domain.Repositories;
using MediatR;
using MongoDB.Driver;

namespace GroceryList.Infrastructure.Repositories;
public class ItemRepository : MongoDbRepositoryBase<Item, Guid>, IItemRepository
{
    public ItemRepository(IMongoDatabase database, IMediator mediator, IDateTimeProvider dateTimeProvider, IClaimReader claimReader) : base(database, mediator, dateTimeProvider, claimReader)
    {
    }
}
