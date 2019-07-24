using BMS.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.DAL.Repositories.GenericRepository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        void Delete(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
        IQueryable<TEntity> AsQueryable();
    }
}
