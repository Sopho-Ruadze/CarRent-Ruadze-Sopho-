using System.ComponentModel.DataAnnotations;

namespace CarRental_API.Models.DTOs.Role
{
    public class RoleUpdateDTO
    {
        [Required]
        public string OldName { get; set; }
        [Required]
        public string NewName { get; set; }
    }
}
