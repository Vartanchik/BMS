using System.ComponentModel.DataAnnotations;

namespace BMS.WebAPI.Models
{
    public class CommentUpdateModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
