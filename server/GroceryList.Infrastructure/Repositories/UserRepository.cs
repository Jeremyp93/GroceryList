using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Users;
using GroceryList.Domain.Helpers.Contracts;
using GroceryList.Domain.Repositories;
using MediatR;
using MongoDB.Driver;

namespace GroceryList.Infrastructure.Repositories;
public class UserRepository : MongoDbRepositoryBase<User, Guid>, IUserRepository
{
    public UserRepository(IMongoDatabase database, IMediator mediator, IDateTimeProvider dateTimeProvider, IClaimReader claimReader) : base(database, mediator, dateTimeProvider, claimReader)
    {
    }
}