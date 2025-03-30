using AutoMapper;
using CarRental_API.Interfaces;
using CarRental_API.Models;
using CarRental_API.Models.DTOs.Car;
using CarRental_API.Models.DTOs.Role;
using CarRental_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental_API.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CarService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<bool>> AddCar(CarAddDTO dto, string userEmail)
        {
            var response = new ServiceResponse<bool>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToUpper() == userEmail.ToUpper());
            var newCar = _mapper.Map<Car>(dto);

            newCar.OwnerId = user;
            newCar.PopularityIndex = 0;

            if (DateTime.Compare(newCar.AvailableDate.Date, DateTime.Now.Date) == 0)
                newCar.IsAvailable = true;
            else
                newCar.IsAvailable = false;

            await _context.Cars.AddAsync(newCar);
            await _context.SaveChangesAsync();

            response.Data = true;
            response.Message = $"Car {newCar.Make} {newCar.Model} has been succesfully added";
            return response;

        }

        public async Task<ServiceResponse<List<CarMinDetailsDTO>>> GetAllAvailableCars()
        {
            var response = new ServiceResponse<List<CarMinDetailsDTO>>();
            var cars = await _context.Cars.Where(x => x.IsAvailable).ToListAsync();
            response.Data = cars.Select(x => _mapper.Map<CarMinDetailsDTO>(x)).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<CarMinDetailsDTO>>> GetAllCars()
        {
            var response = new ServiceResponse<List<CarMinDetailsDTO>>();
            var cars = await _context.Cars.ToListAsync();
            response.Data = cars.Select(x => _mapper.Map<CarMinDetailsDTO>(x)).ToList();
            return response;
        }

        public async Task<ServiceResponse<CarDTO>> GetCar(int id)
        {
            var response = new ServiceResponse<CarDTO>();   
            var car = await _context.Cars.Include(x => x.OwnerId).FirstOrDefaultAsync(x => x.Id == id);
            
            if (car is null)
            {
                response.Success = false;
                response.Message = $"We don't have a car with Id: {id}";
                return response;
            }

            var carDTO = _mapper.Map<CarDTO>(car);
            carDTO.CarOwnerId = car.OwnerId.Id;
            response.Data = carDTO;
            response.Message = $"there is a data of car Id: {id}";
            return response;
        }

        public async Task<ServiceResponse<List<CarDTO>>> GetMyCars(string userEmail)
        {
            var response = new ServiceResponse<List<CarDTO>>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToUpper() == userEmail.ToUpper());
            var ownedCars = await _context.Cars.Where(x => x.OwnerId.Id == user.Id).ToListAsync();

            var carDtoList = new List<CarDTO>();
            ownedCars.ForEach(x => carDtoList.Add(_mapper.Map<CarDTO>(x)));
            carDtoList.ForEach(x => x.CarOwnerId = user.Id);
            response.Data = carDtoList;
            response.Message = $"This is the list of cars owned by {user.Email}";
            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveCar(int id, string userEmail, bool isAdmin)
        {
            var response = new ServiceResponse<bool>();
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToUpper() == userEmail.ToUpper());

            if (car == null)
            {
                response.Success = false;
                response.Message = $"There is no car with ID: {id}";
                return response;
            }
            else if (car.OwnerId.Id != user.Id || !isAdmin)
            {
                response.Success = false;
                response.Message = $"You dont have a permission to remove this car";
                return response;
            }
            else
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                response.Message = "Car was removed successfully";
                response.Data = true;
                return response;
            }
        }

        public async Task<ServiceResponse<CarDTO>> UpdateCar(CarUpdateDTO dto, string userEmail, bool isAdmin)
        {
            var response = new ServiceResponse<CarDTO>();
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == dto.Id);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToUpper() == userEmail.ToUpper());


            if(car == null)
            {
                response.Success = false;
                response.Message = $"There is no car with ID: {dto.Id}";
                return response;
            }
            else if(car.OwnerId.Id != user.Id || !isAdmin)
            {
                response.Success = false;
                response.Message = $"You dont have a permission to update this car";
                return response;
            }
            else
            {
                car = UpdateCarProps(car, dto);
                _context.Cars.Update(car);
                await _context.SaveChangesAsync();
                var updatedCarDTO = _mapper.Map<CarDTO>(car);
                updatedCarDTO.CarOwnerId = car.OwnerId.Id;
                response.Data = updatedCarDTO;
                response.Message = $"Car with ID: {car.Id} has been succesfully updated";
                return response;
            }
        }

        #region PrivateMethods
        public Car UpdateCarProps(Car car, CarUpdateDTO dto)
        {
            if (dto.Make != null)
                car.Make = dto.Make;
            if (dto.Model != null)
                car.Model = dto.Model;
            if (dto.Year.HasValue)
                car.Year = dto.Year.Value;
            if (dto.Transmission.HasValue)
                car.Transmission = dto.Transmission.Value;
            if (dto.EngineDisplacement.HasValue)
                car.EngineDisplacement = dto.EngineDisplacement.Value;
            if (dto.HorsePower.HasValue)
                car.HorsePower = dto.HorsePower.Value;
            if (dto.Drivetrain.HasValue)
                car.Drivetrain = dto.Drivetrain.Value;
            if (dto.Color != null)
                car.Color = dto.Color;
            if (dto.PassengerCapacity.HasValue)
                car.PassengerCapacity = dto.PassengerCapacity.Value;
            if (dto.Location != null)
                car.Location = dto.Location;
            if (dto.Images != null && dto.Images.Any())
                car.Images = dto.Images;
            if (dto.DailyPrice.HasValue)
                car.DailyPrice = dto.DailyPrice.Value;
            if (dto.FuelType.HasValue)
                car.FuelType = dto.FuelType.Value;
            if (dto.FuelTankCapacity.HasValue)
                car.FuelTankCapacity = dto.FuelTankCapacity.Value;
            if (dto.FuelConsumptionInUrban.HasValue)
                car.FuelConsumptionInUrban = dto.FuelConsumptionInUrban.Value;
            if (dto.FuelConsumptionInHighway.HasValue)
                car.FuelConsumptionInHighway = dto.FuelConsumptionInHighway.Value;
            return car;
        }
        #endregion
    }
}