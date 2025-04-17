using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Diplom.Models
{
    public class EditProfileViewModel
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Login { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }
    }
}
