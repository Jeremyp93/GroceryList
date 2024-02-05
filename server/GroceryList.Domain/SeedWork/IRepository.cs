using System.Linq.Expressions;

namespace GroceryList.Domain.SeedWork
{
    public interface IRepository<TAggregateRoot, TId> where TAggregateRoot : AggregateRoot
    {
        Task<IEnumerable<TAggregateRoot>> GetAllAsync(bool filterByUser = true, CancellationToken cancellationToken = default);

        Task<TAggregateRoot?> GetByIdAsync(TId id);

        IQueryable<TAggregateRoot> FindQueryable(Expression<Func<TAggregateRoot, bool>> expression, bool filterByUser = true, Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>>? orderBy = null);

        Task<IEnumerable<TAggregateRoot>> WhereAsync(Expression<Func<TAggregateRoot, bool>>? expression, bool filterByUser = true, Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>>? orderBy = null, CancellationToken cancellationToken = default);

        Task<TAggregateRoot?> SingleOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> expression, bool filterByUser = true);

        Task<TAggregateRoot> AddAsync(TAggregateRoot entity);

        Task UpdateAsync(TAggregateRoot entity);

        Task UpdateRangeAsync(IEnumerable<TAggregateRoot> entities);

        Task DeleteAsync(TAggregateRoot entity);
    }
}
