using CarRental_API.Models;
using CarRental_API.Models.DTOs.Auth;

namespace CarRental_API.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegisterDTO userRegisterDTO);
        Task<ServiceResponse<string>> Login(UserLoginDTO userLoginDTO);
    }
}