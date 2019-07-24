namespace BMS.BLL.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public int PostId { get; set; }
    }
}
