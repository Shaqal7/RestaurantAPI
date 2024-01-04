using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Services
{
	public class AccountService : IAccountService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IPasswordHasher<User> _passwordHasher;
		private readonly AuthenticationSettings _authenticationSettings;

		public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
		{
			_dbContext = dbContext;
			_passwordHasher = passwordHasher;
			_authenticationSettings = authenticationSettings;
		}
		public void RegisterUser(RegisterUserDto dto)
		{
			var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);
			if (existingUser != null)
			{
				throw new BadRequestException("User with this email already exists");
			}

			User newUser = new()
			{
				Email = dto.Email,
				DateOfBirth = dto.DateOfBirth,
				Nationality = dto.Nationality,
				RoleId = dto.RoleId
			};

			var hashedPassword =  _passwordHasher.HashPassword(newUser, dto.Password);

			newUser.PasswordHash = hashedPassword;
			_dbContext.Users.Add(newUser);
			_dbContext.SaveChanges();
		}

		public string GenerateJwt(LoginUserDto dto)
		{
			var existingUser = _dbContext
				.Users
				.Include(u => u.Role)
				.FirstOrDefault(u => u.Email == dto.Email);

			if (existingUser is null)
			{
                throw new BadRequestException("Invalid username or password");
			}
			else
			{
				var result = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, dto.Password);

				if (result == PasswordVerificationResult.Failed)
				{
					throw new BadRequestException("Invalid username or password");
				}

				List<Claim> claims = new List<Claim>()
				{
					new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
					new Claim(ClaimTypes.Name, $"{existingUser.FirstName} {existingUser.LastName}"),
					new Claim(ClaimTypes.Role, $"{existingUser.Role.Name}"),
					new Claim("DateOfBirth", existingUser.DateOfBirth.Value.ToString("yyyy-MM-dd")),
				};

				if (!string.IsNullOrEmpty(existingUser.Nationality))
				{
					claims.Add(
						new Claim("Nationality", existingUser.Nationality)
						);
				}

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
				var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

				var token = new JwtSecurityToken(
					_authenticationSettings.JwtIssuer,
					_authenticationSettings.JwtIssuer,
					claims,
					expires: expires,
					signingCredentials: credentials
					);

				var tokenHandler = new JwtSecurityTokenHandler();

				return tokenHandler.WriteToken(token);
			}
		}
	}
}
