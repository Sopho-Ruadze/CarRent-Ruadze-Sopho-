namespace CarRental_API.Models.DTOs.CarRent
{
    public class CarRentDetailsDTO
    {
        public int CarId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int DailyPrice { get; set; }
        public int TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int TotalDays { get; set; }
        public int RenterId { get; set; }
    }
}