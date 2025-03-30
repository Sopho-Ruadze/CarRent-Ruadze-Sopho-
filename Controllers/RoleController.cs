using CarRental_API.Interfaces;
using CarRental_API.Models;
using CarRental_API.Models.DTOs.Role;
using CarRental_API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<ServiceResponse<List<RoleDTO>>>> GetAllAsync()
        {
            var result = await _roleService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<RoleDTO>>> GetByIdAsync(int id)
        {
            var result = await _roleService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ServiceResponse<string>>> CreateAsync([FromForm] RoleCreateDTO dto)
        {
            var result = await _roleService.CreateAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateAsync([FromForm] RoleUpdateDTO dto)
        {
            var result = await _roleService.UpdateAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("DeleteById")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteByIdAsync([FromForm] int id)
        {
            var result = await _roleService.DeleteByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("DeleteByName")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteByNameAsync([FromForm] string name)
        {
            var result = await _roleService.DeleteByNameAsync(name);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
