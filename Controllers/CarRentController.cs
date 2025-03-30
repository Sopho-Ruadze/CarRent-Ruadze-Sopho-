using CarRental_API.Models.DTOs.CarRent;
using CarRental_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CarRental_API.Interfaces;

namespace CarRental_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarRentController : ControllerBase
    {
        private readonly ICarRentService _carRentService;

        public CarRentController(ICarRentService carRentService)
        {
            _carRentService = carRentService;
        }

        [HttpPost("RentCarById")]
        public async Task<ActionResult<ServiceResponse<CarRentDetailsDTO>>> RentCar(int id, int days = 1)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var dto = new CarRentDTO()
            {
                UserEmail = userEmail,
                CarId = id,
                Days = days
            };
            var result = await _carRentService.RentCar(dto);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ReturnCarById")]
        public async Task<ActionResult<ServiceResponse<bool>>> ReturnCar(int carId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await _carRentService.ReturnCar(carId, userEmail);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
