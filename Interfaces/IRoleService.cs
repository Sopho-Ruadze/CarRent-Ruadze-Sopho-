using CarRental_API.Models;
using CarRental_API.Models.DTOs.Role;
using CarRental_API.Models.Entities;

namespace CarRental_API.Interfaces
{
    public interface IRoleService
    {
        Task<ServiceResponse<List<RoleDTO>>> GetAllAsync();
        Task<ServiceResponse<RoleDTO>> GetByIdAsync(int id);
        Task<ServiceResponse<string>> CreateAsync(RoleCreateDTO dto);
        Task<ServiceResponse<string>> UpdateAsync(RoleUpdateDTO dto);
        Task<ServiceResponse<string>> DeleteByIdAsync(int id);
        Task<ServiceResponse<string>> DeleteByNameAsync(string name);
    }
}