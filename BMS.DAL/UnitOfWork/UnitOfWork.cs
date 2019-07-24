using BMS.DAL.Entities;
using BMS.DAL.Repositories.ConcreteRepositories.AppUserRepository;
using BMS.DAL.Repositories.GenericRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace BMS.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private IAppUserRepository<AppUser> _appUsers;
        private IRepository<Post> _posts;
        private IRepository<Comment> _comments;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IAppUserRepository<AppUser> AppUsers => _appUsers ?? (_appUsers = new AppUserRepository(_context, _userManager));
        public IRepository<Post> Posts => _posts ?? (_posts = new RepositoryBase<Post>(_context));
        public IRepository<Comment> Comments => _comments ?? (_comments = new RepositoryBase<Comment>(_context));

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
