using BMS.DAL.Entities;
using BMS.DAL.Repositories.ConcreteRepositories.AppUserRepository;
using BMS.DAL.Repositories.GenericRepository;
using System;
using System.Threading.Tasks;

namespace BMS.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAppUserRepository<AppUser> AppUsers { get; }
        IRepository<Post> Posts { get; }
        IRepository<Comment> Comments { get; }

        void Commit();

        Task CommitAsync();
    }
}
