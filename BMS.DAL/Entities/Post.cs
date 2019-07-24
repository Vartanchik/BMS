using System.Collections.Generic;

namespace BMS.DAL.Entities
{
    public class Post : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public AppUser Author { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
