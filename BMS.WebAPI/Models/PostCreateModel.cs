using System.ComponentModel.DataAnnotations;

namespace BMS.WebAPI.Models
{
    public class PostCreateModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
