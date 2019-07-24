using System.ComponentModel.DataAnnotations;

namespace BMS.WebAPI.Models
{
    public class PostUpdateModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
