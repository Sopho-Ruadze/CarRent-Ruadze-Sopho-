using System.ComponentModel.DataAnnotations;

namespace CarRental_API.Models.DTOs.Auth
{
    public class UserLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
