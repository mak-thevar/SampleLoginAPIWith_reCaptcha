using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleLoginAPI.Data;
using SampleLoginAPI.DTOs;
using SampleLoginAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleLoginAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;
        }

        public async Task<(bool IsSuccess, string Message)> RegisterUserAsync(UserRegisterDto dto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Username == dto.Username))
                return (false, "Username already exists.");

            var user = new User
            {
                Username = dto.Username
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return (true, "User registered successfully.");
        }

        public async Task<(bool IsSuccess, string Message)> LoginUserAsync(UserLoginDto dto)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
                return (false, "Invalid username or password.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result != PasswordVerificationResult.Success)
                return (false, "Invalid username or password.");

            var token = GenerateJwtToken(user);
            return (true, token);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(int.Parse(jwtSettings["TokenLifetimeInHours"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
