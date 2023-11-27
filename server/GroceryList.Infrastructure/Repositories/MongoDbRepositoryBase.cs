using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Helpers.Contracts;
using GroceryList.Domain.SeedWork;
using GroceryList.Infrastructure.Extension;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GroceryList.Infrastructure.Repositories;

public class MongoDbRepositoryBase<TAggregateRoot, TId> : IRepository<TAggregateRoot, TId> where TAggregateRoot : AggregateRoot
{
    private readonly IMongoCollection<TAggregateRoot> _collection;
    private readonly IMediator _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMongoDatabase _database;
    public MongoDbRepositoryBase(IMongoDatabase database, IMediator mediator,
    IDateTimeProvider dateTimeProvider)
    {
        _collection = database.GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
        _database = database;
    }

    public async Task<IEnumerable<TAggregateRoot>> GetAllAsync(CancellationToken cancellationToken)
    {
        var filter = Builders<TAggregateRoot>.Filter.Empty;
        var result = await _collection.Find(filter).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<TAggregateRoot?> GetByIdAsync(TId id)
    {
        var filter = Builders<TAggregateRoot>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public IQueryable<TAggregateRoot> FindQueryable(Expression<Func<TAggregateRoot, bool>> expression, Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>>? orderBy = null)
    {
        var query = _collection.AsQueryable().Where(expression);
        return orderBy != null ? orderBy(query) : query;
    }

    public async Task<IEnumerable<TAggregateRoot>> WhereAsync(Expression<Func<TAggregateRoot, bool>>? expression, Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>>? orderBy = null, CancellationToken cancellationToken = default)
    {
        var query = expression != null ? _collection.AsQueryable().Where(expression) : _collection.AsQueryable();
        query = orderBy != null ? orderBy(query) : query;

        var result = await _collection.FindAsync(expression);
        return result.ToEnumerable();
    }

    public async Task<TAggregateRoot?> SingleOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> expression, string includeProperties)
    {
        var filter = Builders<TAggregateRoot>.Filter.Where(expression);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<TAggregateRoot> AddAsync(TAggregateRoot entity)
    {
        entity.UpdateTrackingInformation("fake-auth-user-todo", _dateTimeProvider.UtcNow);

        await _collection.InsertOneAsync(entity);

        await _mediator.DispatchDomainEventsAsync(entity);

        return entity;
    }

    public async Task UpdateAsync(TAggregateRoot entity)
    {
        entity.UpdateTrackingInformation("fake-auth-user-todo", _dateTimeProvider.UtcNow);

        var filter = Builders<TAggregateRoot>.Filter.Eq("_id", entity.Id);
        await _collection.ReplaceOneAsync(filter, entity);

        await _mediator.DispatchDomainEventsAsync(entity);
    }

    /* [TODO]: to be improved - probably not the most efficient option */
    public async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> entities)
    {
        foreach (var entity in entities)
        {
            entity.UpdateTrackingInformation("fake-auth-user-todo", _dateTimeProvider.UtcNow);
            await UpdateAsync(entity);
        }

        await _mediator.DispatchDomainEventsAsync(entities);
    }

    public async Task DeleteAsync(TAggregateRoot entity)
    {
        var filter = Builders<TAggregateRoot>.Filter.Eq("_id", entity.Id);
        await _collection.DeleteOneAsync(filter);

        await _mediator.DispatchDomainEventsAsync(entity);
    }
}