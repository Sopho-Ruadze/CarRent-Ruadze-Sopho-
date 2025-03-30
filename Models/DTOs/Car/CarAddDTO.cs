using CarRental_API.Enums;
using CarRental_API.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace CarRental_API.Models.DTOs.Car
{
    public class CarAddDTO
    {
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public Transmission Transmission { get; set; }
        [Required]
        public decimal EngineDisplacement { get; set; }
        [Required]
        public int HorsePower { get; set; }
        [Required]
        public Drivetrain Drivetrain { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public int PassengerCapacity { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime AvailableDate { get; set; }
        [Required]
        public List<string> Images { get; set; }
        [Required]
        public decimal DailyPrice { get; set; }
        [Required]
        public FuelType FuelType { get; set; }
        [Required]
        public decimal FuelTankCapacity { get; set; }
        [Required]
        public decimal FuelConsumptionInUrban { get; set; }
        [Required]
        public decimal FuelConsumptionInHighway { get; set; }
        //public User OwnerId { get; set; }
    }
}