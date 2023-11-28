using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Helpers.Contracts;
using GroceryList.Domain.Repositories;
using MediatR;
using MongoDB.Driver;

namespace GroceryList.Infrastructure.Repositories;
public class StoreRepository : MongoDbRepositoryBase<Store, Guid>, IStoreRepository
{
    public StoreRepository(IMongoDatabase database, IMediator mediator, IDateTimeProvider dateTimeProvider, IClaimReader claimReader) : base(database, mediator, dateTimeProvider, claimReader)
    {
    }
}