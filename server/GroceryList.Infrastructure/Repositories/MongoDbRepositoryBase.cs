using GroceryList.Application.Abstractions;
using GroceryList.Domain.Helpers.Contracts;
using GroceryList.Domain.SeedWork;
using GroceryList.Infrastructure.Extension;
using MediatR;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace GroceryList.Infrastructure.Repositories;

public class MongoDbRepositoryBase<TAggregateRoot, TId> : IRepository<TAggregateRoot, TId> where TAggregateRoot : AggregateRoot
{
    private readonly string _username;
    private readonly IMongoCollection<TAggregateRoot> _collection;
    private readonly IMediator _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private static Guid _userId;

    public MongoDbRepositoryBase(IMongoDatabase database, IMediator mediator,
    IDateTimeProvider dateTimeProvider, IClaimReader claimReader)
    {
        _collection = database.GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
        _username = claimReader.GetUsernameFromClaim();
        _userId = claimReader.GetUserIdFromClaim();
    }

    public async Task<IEnumerable<TAggregateRoot>> GetAllAsync(bool filterByUser = true, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TAggregateRoot>.Filter.Empty;
        if (filterByUser) 
        { 
            filter = Builders<TAggregateRoot>.Filter.Eq("userId", _userId);
        }
        
        var result = await _collection.Find(filter).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<TAggregateRoot?> GetByIdAsync(TId id)
    {
        var filter = Builders<TAggregateRoot>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public IQueryable<TAggregateRoot> FindQueryable(Expression<Func<TAggregateRoot, bool>> expression, bool filterByUser = true, Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>>? orderBy = null)
    {
        var definitiveExpression = expression;
        if (filterByUser)
        {
            definitiveExpression = AddUserCondition(expression);
        }
        var query = _collection.AsQueryable().Where(definitiveExpression);
        return orderBy != null ? orderBy(query) : query;
    }

    public async Task<IEnumerable<TAggregateRoot>> WhereAsync(Expression<Func<TAggregateRoot, bool>>? expression, bool filterByUser = true, Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>>? orderBy = null, CancellationToken cancellationToken = default)
    {
        var definitiveExpression = expression;
        if (filterByUser)
        {
            definitiveExpression = AddUserCondition(expression);
        }
        var query = definitiveExpression != null ? _collection.AsQueryable().Where(definitiveExpression) : _collection.AsQueryable();
        _ = orderBy != null ? orderBy(query) : query;

        var result = await _collection.FindAsync(definitiveExpression, cancellationToken: cancellationToken);
        return result.ToEnumerable(cancellationToken: cancellationToken);
    }

    public async Task<TAggregateRoot?> SingleOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> expression, bool filterByUser = true)
    {
        var definitiveExpression = expression;
        if (filterByUser)
        {
            definitiveExpression = AddUserCondition(expression);
        }
        var filter = Builders<TAggregateRoot>.Filter.Where(definitiveExpression);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<TAggregateRoot> AddAsync(TAggregateRoot entity)
    {
        entity.UpdateTrackingInformation(_username, _dateTimeProvider.UtcNow);
        entity.AddUser(_userId);

        await _collection.InsertOneAsync(entity);

        await _mediator.DispatchDomainEventsAsync(entity);

        return entity;
    }

    public async Task UpdateAsync(TAggregateRoot entity)
    {
        entity.UpdateTrackingInformation(_username, _dateTimeProvider.UtcNow);

        var filter = Builders<TAggregateRoot>.Filter.Eq("_id", entity.Id);
        await _collection.ReplaceOneAsync(filter, entity);

        await _mediator.DispatchDomainEventsAsync(entity);
    }

    /* [TODO]: to be improved - probably not the most efficient option */
    public async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> entities)
    {
        var bulkOperations = new List<WriteModel<TAggregateRoot>>();

        foreach (var entity in entities)
        {
            entity.UpdateTrackingInformation(_username, _dateTimeProvider.UtcNow);

            var filter = Builders<TAggregateRoot>.Filter.Eq("_id", entity.Id); // Assuming _id is the identifier field

            var replaceOneModel = new ReplaceOneModel<TAggregateRoot>(filter, entity)
            {
                IsUpsert = false // Set to true if you want to upsert
            };

            bulkOperations.Add(replaceOneModel);
        }

        var options = new BulkWriteOptions { IsOrdered = false }; // Set IsOrdered as needed

        if (bulkOperations.Any())
        {
            await _collection.BulkWriteAsync(bulkOperations, options);
        }

        await _mediator.DispatchDomainEventsAsync(entities);
    }

    public async Task DeleteAsync(TAggregateRoot entity)
    {
        var filter = Builders<TAggregateRoot>.Filter.Eq("_id", entity.Id);
        await _collection.DeleteOneAsync(filter);

        await _mediator.DispatchDomainEventsAsync(entity);
    }

    private static Expression<Func<TAggregateRoot, bool>> AddUserCondition(Expression<Func<TAggregateRoot, bool>>? existingExpression)
    {
        Expression<Func<TAggregateRoot, bool>> additionalCondition = entity => entity.UserId == _userId;

        if (existingExpression == null)
        {
            return additionalCondition;
        }

        var combinedExpression = Expression.Lambda<Func<TAggregateRoot, bool>>(
            Expression.AndAlso(
                existingExpression.Body,
                additionalCondition.Body.ReplaceParameter(additionalCondition.Parameters[0], existingExpression.Parameters[0])
            ),
            existingExpression.Parameters
        );

        return combinedExpression;
    }
}