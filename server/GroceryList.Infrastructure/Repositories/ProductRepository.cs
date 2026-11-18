using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Products;
using GroceryList.Domain.Helpers.Contracts;
using GroceryList.Domain.Repositories;
using MediatR;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace GroceryList.Infrastructure.Repositories;

public class ProductRepository : MongoDbRepositoryBase<Product, Guid>, IProductRepository
{
    public ProductRepository(IMongoDatabase database, IMediator mediator, IDateTimeProvider dateTimeProvider, IClaimReader claimReader) : base(database, mediator, dateTimeProvider, claimReader)
    {
        _collection = database.GetCollection<Product>("colruyt_data");
    }

    public new Task<Product?> GetByIdAsync(Guid id) { throw new NotImplementedException(); }
    public new IQueryable<Product> FindQueryable(Expression<Func<Product, bool>> expression, Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null) { throw new NotImplementedException(); }
    public new Task<Product?> SingleOrDefaultAsync(Expression<Func<Product, bool>> expression) { throw new NotImplementedException(); }
    public new Task<Product> AddAsync(Product entity) { throw new NotImplementedException(); }
    public new Task UpdateAsync(Product entity) { throw new NotImplementedException(); }
    public new Task UpdateRangeAsync(IEnumerable<Product> entities) { throw new NotImplementedException(); }
    public new Task DeleteAsync(Product entity) { throw new NotImplementedException(); }
}
