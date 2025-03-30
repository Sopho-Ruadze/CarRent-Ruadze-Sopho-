using CarRental_API.Interfaces;
using CarRental_API.Models;
using CarRental_API.Models.DTOs.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRental_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }


        [Authorize]
        [HttpPost("AddCarFromForm")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddCarFromForm([FromForm] CarAddDTO dto)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await _carService.AddCar(dto, userEmail);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpPost("AddCar")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddCar(CarAddDTO dto)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await _carService.AddCar(dto, userEmail);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpPut("UpdateCar")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateCar(CarUpdateDTO dto)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var isAdmin = User.IsInRole("Admin");
            var result = await _carService.UpdateCar(dto, userEmail, isAdmin);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetCarById")]
        public async Task<ActionResult<ServiceResponse<CarDTO>>> GetCar(int id)
        {
            var result = await _carService.GetCar(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("GetMyCars")]
        public async Task<ActionResult<ServiceResponse<List<CarDTO>>>> GetMyCars()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await _carService.GetMyCars(userEmail);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllCars")]
        public async Task<ActionResult<ServiceResponse<List<CarMinDetailsDTO>>>> GetAllCars()
        {
            var result = await _carService.GetAllCars();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllAvailableCars")]
        public async Task<ActionResult<ServiceResponse<List<CarMinDetailsDTO>>>> GetAllAvailableCars()
        {
            var result = await _carService.GetAllAvailableCars();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpDelete("DeleteCarById")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveCarById(int id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var isAdmin = User.IsInRole("Admin");
            var result = await _carService.RemoveCar(id, userEmail, isAdmin);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}