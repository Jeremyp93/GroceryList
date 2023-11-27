using GroceryList.Domain.Helpers.Contracts;
using GroceryList.Domain.Repositories;
using MediatR;
using MongoDB.Driver;

namespace GroceryList.Infrastructure.Repositories;
public class GroceryListRepository : MongoDbRepositoryBase<Domain.Aggregates.GroceryLists.GroceryList, Guid>, IGroceryListRepository
{
    public GroceryListRepository(IMongoDatabase database, IMediator mediator, IDateTimeProvider dateTimeProvider) : base(database, mediator, dateTimeProvider)
    {

    }
}