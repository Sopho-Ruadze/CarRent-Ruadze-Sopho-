using AutoMapper;
using CarRental_API.Interfaces;
using CarRental_API.Models;
using CarRental_API.Models.DTOs.Role;
using CarRental_API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;

namespace CarRental_API.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> CreateAsync(RoleCreateDTO dto)
        {
            var result = new ServiceResponse<string>();

            if (await RoleExists(dto.Name))
            {
                result.Success = false;
                result.Message = $"Role with name: {dto.Name} already exists.";
                return result;
            }

            var newRole = new Role(dto.Name);

            await _context.Roles.AddAsync(newRole);
            await _context.SaveChangesAsync();

            result.Message = $"Role with name: {newRole.Name} has been added succesfully!";
            return result;
        }

        public async Task<ServiceResponse<string>> DeleteByIdAsync(int id)
        {
            var result = new ServiceResponse<string>();
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (role is null)
            {
                result.Success = false;
                result.Message = $"Role with ID: {id} does not exists.";
                return result;
            }

            if (role.Name == "Admin")
            {
                result.Success = false;
                result.Message = "You don't have a permission to remove this role.";
                return result;
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            result.Message = $"Role with ID: {id} has been succesfully deleted";
            return result;
        }

        public async Task<ServiceResponse<string>> DeleteByNameAsync(string name)
        {
            var result = new ServiceResponse<string>();
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name.ToUpper() == name.ToUpper());

            if (role is null)
            {
                result.Success = false;
                result.Message = $"Role with name: {name} does not exists.";
                return result;
            }

            if (role.Name == "Admin")
            {
                result.Success = false;
                result.Message = "You don't have a permission to remove this role.";
                return result;
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            result.Message = $"Role with name: {name} has been succesfully deleted";
            return result;
        }

        public async Task<ServiceResponse<List<RoleDTO>>> GetAllAsync()
        {
            var result = new ServiceResponse<List<RoleDTO>>();

            var roles = await _context.Roles.ToListAsync();
            
            if (roles.Count == 0)
            {
                result.Success = false;
                result.Message = "No roles were found.";
                return result;
            }

            result.Data = roles.Select(x => _mapper.Map<RoleDTO>(x)).ToList();
            result.Message = "Roles were succesfully fetched from database";
            return result;
        }

        public async Task<ServiceResponse<RoleDTO>> GetByIdAsync(int id)
        {
            var result = new ServiceResponse<RoleDTO>();

            var role = _context.Roles.FirstOrDefault(x => x.Id == id);

            if (role is null)
            {
                result.Success = false;
                result.Message = $"There is no role with this ID: {id}";
                return result;
            }

            result.Data = _mapper.Map<RoleDTO>(role);
            result.Message = "Role was succesfully found in database";
            return result;
        }

        public async Task<ServiceResponse<string>> UpdateAsync(RoleUpdateDTO dto)
        {
            var result = new ServiceResponse<string>();
            
            var role = _context.Roles.FirstOrDefault(x => x.Name.ToUpper() == dto.OldName.ToUpper());

            if (role is null)
            {
                result.Success = false;
                result.Message = $"There is no role with name: {dto.OldName} to update.";
                return result;
            }
            if (await RoleExists(dto.NewName))
            {
                result.Success = false;
                result.Message = $"There is already a role with name: {dto.NewName}.";
                return result;
            }

            role.Name = dto.NewName;
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();

            result.Message = $"The {dto.OldName} role was updated and now its name is: {dto.NewName}";
            return result;
        }



        #region PrivateMethods
        public async Task<bool> RoleExists(string name) => 
            await _context.Roles.AnyAsync(x => x.Name.ToUpper() == name.ToUpper());


        #endregion
    }
}
