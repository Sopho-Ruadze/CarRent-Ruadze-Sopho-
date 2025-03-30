using CarRental_API.Models;
using CarRental_API.Models.DTOs.CarRent;

namespace CarRental_API.Interfaces
{
    public interface ICarRentService
    {
        Task<ServiceResponse<CarRentDetailsDTO>> RentCar(CarRentDTO dto);
        Task<ServiceResponse<bool>> ReturnCar(int carId, string userEmail);
        Task<ServiceResponse<List<CarRentDetailsDTO>>> GetMyRentedCarsDetails(string userEmail);
    }
}