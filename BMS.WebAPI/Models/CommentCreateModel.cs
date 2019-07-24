using System.ComponentModel.DataAnnotations;

namespace BMS.WebAPI.Models
{
    public class CommentCreateModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int PostId { get; set; }
    }
}
