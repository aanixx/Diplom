using System.ComponentModel.DataAnnotations;

namespace Diplom.Models
{
    public class RegViewModel
    {
        [Required]
        public string? Login { get; set; }

        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
        public string? ConfirmPassword { get; set; }
    }
}
