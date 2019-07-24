using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BMS.DAL.Entities
{
    public class AppUser : IdentityUser<int>, IEntity
    {
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
