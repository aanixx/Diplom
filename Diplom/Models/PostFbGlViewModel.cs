using System.ComponentModel.DataAnnotations;

namespace Diplom.Models
{
    public class PostFbGlViewModel
    {
        public string? Id { get; set; }

        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Login { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}
