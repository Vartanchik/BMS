using BMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.DAL.Repositories.GenericRepository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;

            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var entityToUpdate = await _dbSet.FindAsync(entity.Id);

            var attachedEntry = _context.Entry(entityToUpdate);

            attachedEntry.CurrentValues.SetValues(entity);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }


        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsQueryable<TEntity>();
        }
    }
}
