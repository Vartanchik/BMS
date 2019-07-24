namespace BMS.DAL.Entities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public AppUser Author { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
