namespace CarRental_API.Models.DTOs.Car
{
    public class CarMinDetailsDTO
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public bool IsAvailable { get; set; }

    }
}