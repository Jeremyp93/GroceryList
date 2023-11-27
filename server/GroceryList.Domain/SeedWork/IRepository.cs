using System.Linq.Expressions;

namespace GroceryList.Domain.SeedWork
{
    public interface IRepository<TAggregateRoot, TId> where TAggregateRoot : AggregateRoot
    {
        Task<IEnumerable<TAggregateRoot>> GetAllAsync(CancellationToken cancellationToken);

        Task<TAggregateRoot?> GetByIdAsync(TId id);

        IQueryable<TAggregateRoot> FindQueryable(Expression<Func<TAggregateRoot, bool>> expression, Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>>? orderBy = null);

        Task<IEnumerable<TAggregateRoot>> WhereAsync(Expression<Func<TAggregateRoot, bool>>? expression, Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>>? orderBy = null, CancellationToken cancellationToken = default);

        Task<TAggregateRoot?> SingleOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> expression, string includeProperties);

        Task<TAggregateRoot> AddAsync(TAggregateRoot entity);

        Task UpdateAsync(TAggregateRoot entity);

        Task UpdateRangeAsync(IEnumerable<TAggregateRoot> entities);

        Task DeleteAsync(TAggregateRoot entity);
    }
}
