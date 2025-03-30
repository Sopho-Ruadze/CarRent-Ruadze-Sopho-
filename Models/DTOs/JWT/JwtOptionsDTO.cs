namespace CarRental_API.Models.DTOs.JWT
{
    public class JwtOptionsDTO
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int AccessTokenExpirationDays { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}
