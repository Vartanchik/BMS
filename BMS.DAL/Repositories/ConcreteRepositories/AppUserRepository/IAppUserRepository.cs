using BMS.DAL.Entities;
using BMS.DAL.Repositories.GenericRepository;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BMS.DAL.Repositories.ConcreteRepositories.AppUserRepository
{
    public interface IAppUserRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IdentityResult> CreateAsync(AppUser appUser, string password);
        Task<bool> CheckPasswordAsync(int userId, string password);
    }
}
