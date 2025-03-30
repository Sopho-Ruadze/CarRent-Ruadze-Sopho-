using CarRental_API.Enums;
using CarRental_API.Models.Entities;

namespace CarRental_API.Models.DTOs.Car
{
    public class CarDTO
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public Transmission Transmission { get; set; }
        public decimal EngineDisplacement { get; set; }
        public int HorsePower { get; set; }
        public Drivetrain Drivetrain { get; set; }
        public string Color { get; set; }
        public int PassengerCapacity { get; set; }
        public string Location { get; set; }
        public List<string> Images { get; set; }
        public decimal DailyPrice { get; set; }
        public FuelType FuelType { get; set; }
        public decimal FuelTankCapacity { get; set; }
        public decimal FuelConsumptionInUrban { get; set; }
        public decimal FuelConsumptionInHighway { get; set; }
        public int PopularityIndex { get; set; }
        public int CarOwnerId { get; set; }
    }
}