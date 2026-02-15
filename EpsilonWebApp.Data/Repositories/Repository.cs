using EpsilonWebApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EpsilonWebApp.Data.Repositories
{
    /// <summary>
    /// Implementation of the generic repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>The database context.</summary>
        protected readonly DbContext Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public Repository(DbContext context)
        {
            Context = context;
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        /// <inheritdoc/>
        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        /// <inheritdoc/>
        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        /// <inheritdoc/>
        public void Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
        }

        /// <inheritdoc/>
        public async Task<int> CountAsync()
        {
            return await Context.Set<TEntity>().CountAsync();
        }
    }
}
