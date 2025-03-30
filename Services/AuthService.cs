using CarRental_API.Interfaces;
using CarRental_API.Models;
using CarRental_API.Models.DTOs.Auth;
using CarRental_API.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarRental_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(UserLoginDTO userLoginDTO)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Email.ToUpper() == userLoginDTO.Email.ToUpper());

            if (user == null) 
            {
                response.Success = false;
                response.Message = "User was not found with this email";
            }
            else if (!VerifyPassword(userLoginDTO.Password, user.PasswordSalt, user.PasswordHash))
            {
                response.Success = false;
                response.Message = $"Incorrect password for this email: {user.Email}";
            }
            else
            {
                var tokenModel = GenerateTokens(user, userLoginDTO.RememberMe);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                response.Data = tokenModel.AccessToken;
                response.Message = "User logged in succesfully";
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(UserRegisterDTO userRegisterDTO)
        {
            var response = new ServiceResponse<int>();

            if (await EmailExists(userRegisterDTO.Email))
            {
                response.Success = false;
                response.Message = "User with this email already exists.";
            }

            CreatePasswordHash(userRegisterDTO.Password, out byte[] passwordSalt, out byte[] passwordHash);

            var user = new User()
            {
                Email = userRegisterDTO.Email,
                FirstName = userRegisterDTO.FirstName,
                LastName = userRegisterDTO.LastName,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash
            };
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(x => x.Name == "Customer");
            user.Roles.Add(defaultRole);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            response.Message = "User has been registered succesfully";
            response.Data = user.Id;
            return response;
        }

        #region PrivateMethods
        public async Task<bool> EmailExists(string email)
            => await _context.Users.AnyAsync(x => x.Email.ToUpper() == email.ToUpper());

        public void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPassword(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public TokenDTO GenerateTokens(User user, bool rememberMe = false)
        {
            var tokenModel = new TokenDTO();

            if (rememberMe)
            {
                tokenModel.RefreshToken = GenerateRefreshToken(user);
                user.RefreshToken = tokenModel.RefreshToken;
                user.RefreshTokenExpirationDate = DateTime.Now.AddDays(7);
            }
            tokenModel.AccessToken = GenerateAccessToken(user);

            return tokenModel;
        }

        public string GenerateAccessToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var userRoleNames = user.Roles.Select(x => x.Name).ToList();
            userRoleNames.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTOptions:Secret").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
                Issuer = _configuration.GetSection("JWTOptions:Issuer").Value,
                Audience = _configuration.GetSection("JWTOptions:Audience").Value
            };

            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateToken(securityTokenDescriptor);

            return handler.WriteToken(token);
        }
        public string GenerateRefreshToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTOptions:Secret").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials,
                Issuer = _configuration.GetSection("JWTOptions:Issuer").Value,
                Audience = _configuration.GetSection("JWTOptions:Audience").Value
            };

            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateToken(securityTokenDescriptor);

            return handler.WriteToken(token);
        }
        #endregion
    }
}