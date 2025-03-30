using AutoMapper;
using CarRental_API.Models.DTOs.Car;
using CarRental_API.Models.DTOs.CarRent;
using CarRental_API.Models.DTOs.Role;
using CarRental_API.Models.Entities;

namespace CarRental_API
{
    public class MappinProfile : Profile
    {
        public MappinProfile()
        {
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Car, CarAddDTO>().ReverseMap();
            CreateMap<Car, CarUpdateDTO>().ReverseMap();
            CreateMap<Car, CarDTO>().ReverseMap();
            CreateMap<Car, CarRentDetailsDTO>().ReverseMap();
            CreateMap<Car, CarMinDetailsDTO>().ReverseMap();
        }
    }
}