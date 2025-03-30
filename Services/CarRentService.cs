using AutoMapper;
using CarRental_API.Interfaces;
using CarRental_API.Models;
using CarRental_API.Models.DTOs.Car;
using CarRental_API.Models.DTOs.CarRent;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CarRental_API.Services
{
    public class CarRentService : ICarRentService
    {
        public readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CarRentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<CarRentDetailsDTO>>> GetMyRentedCarsDetails(string userEmail)
        {
            var response = new ServiceResponse<List<CarRentDetailsDTO>>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToUpper() == userEmail.ToUpper());
            var rentedCars = await _context.Cars.Where(x => x.RenterUserId == user.Id).ToListAsync();

            if (rentedCars.Count == 0)
            {
                response.Success = false;
                response.Message = "There is no cars rented by you";
                return response;
            }


            return response;
        }

        public async Task<ServiceResponse<CarRentDetailsDTO>> RentCar(CarRentDTO dto)
        {
            var response = new ServiceResponse<CarRentDetailsDTO>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToUpper() == dto.UserEmail.ToUpper());
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == dto.CarId);

            if (car is null)
            {
                response.Success = false;
                response.Message = $"We don't have a car with Id: {dto.CarId}";
                return response;
            }
            else if (!car.IsAvailable)
            {
                response.Success = false;
                response.Message = $"This car is not available until {car.AvailableDate}.";
                return response;
            }

            car.RenterUserId = user.Id;
            car.IsAvailable = false;
            car.AvailableDate = DateTime.Now.AddDays(dto.Days);

            var carRentDetailsDto = new CarRentDetailsDTO();
            carRentDetailsDto = _mapper.Map<CarRentDetailsDTO>(car);
            carRentDetailsDto.CarId = car.Id;
            carRentDetailsDto.TotalPrice = (int)(car.DailyPrice * dto.Days);
            carRentDetailsDto.StartDate = DateTime.Now;
            carRentDetailsDto.ReturnDate = DateTime.Now.AddDays(dto.Days);
            carRentDetailsDto.TotalDays = dto.Days;
            carRentDetailsDto.RenterId = user.Id;

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();

            response.Data = carRentDetailsDto;
            response.Message = "You rented car successfully";
            return response;
        }

        public async Task<ServiceResponse<bool>> ReturnCar(int carId, string userEmail)
        {
            var response = new ServiceResponse<bool>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToUpper() == userEmail.ToUpper());
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);


            if (car.RenterUserId != user.Id)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "This is not your rented car's Id.";
                return response;
            }
            
            car.IsAvailable = true;
            car.AvailableDate = DateTime.Now;
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            response.Data = true;
            response.Message = "Car returned succesfully.";
            return response;
        }
    }
}
