using CarRental_API.Interfaces;
using CarRental_API.Models;
using CarRental_API.Models.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register([FromForm] UserRegisterDTO model)
        {
            var result = await _service.Register(model);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login([FromForm] UserLoginDTO model)
        {
            var result = await _service.Login(model);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
