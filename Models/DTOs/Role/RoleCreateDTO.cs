using System.ComponentModel.DataAnnotations;

namespace CarRental_API.Models.DTOs.Role
{
    public class RoleCreateDTO
    {
        [Required]
        public string Name { get; set; }
    }
}