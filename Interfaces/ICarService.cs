using CarRental_API.Models;
using CarRental_API.Models.DTOs.Car;

namespace CarRental_API.Interfaces
{
    public interface ICarService
    {
        Task<ServiceResponse<bool>> AddCar(CarAddDTO dto, string userEmail);
        Task<ServiceResponse<CarDTO>> UpdateCar(CarUpdateDTO dto, string userEmail, bool isAdmin);
        Task<ServiceResponse<CarDTO>> GetCar(int id);
        Task<ServiceResponse<List<CarDTO>>> GetMyCars(string userEmail);
        Task<ServiceResponse<bool>> RemoveCar(int id, string userEmail, bool isAdmin);
        Task<ServiceResponse<List<CarMinDetailsDTO>>> GetAllCars();
        Task<ServiceResponse<List<CarMinDetailsDTO>>> GetAllAvailableCars();
    }
}