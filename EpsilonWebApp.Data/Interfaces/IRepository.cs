using System.Linq.Expressions;

namespace EpsilonWebApp.Data.Interfaces
{
    /// <summary>
    /// Generic repository interface for basic CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>Gets an entity by its unique identifier.</summary>
        Task<TEntity?> GetByIdAsync(Guid id);
        /// <summary>Gets all entities.</summary>
        Task<IEnumerable<TEntity>> GetAllAsync();
        /// <summary>Finds entities based on a predicate.</summary>
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        /// <summary>Adds a new entity asynchronously.</summary>
        Task AddAsync(TEntity entity);
        /// <summary>Removes an entity.</summary>
        void Remove(TEntity entity);
        /// <summary>Updates an existing entity.</summary>
        void Update(TEntity entity);
        /// <summary>Counts the total number of entities.</summary>
        Task<int> CountAsync();
    }
}
