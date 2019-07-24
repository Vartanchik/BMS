using BMS.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BMS.DAL
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(p => p.Author)
                .WithMany(u => u.Comments)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(p => p.Post)
                .WithMany(u => u.Comments)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppUser>().HasData(
                    new AppUser { Id = 1, UserName = "FirstUser", Email = "firstuser@com"}
                );
        }
    }
}
