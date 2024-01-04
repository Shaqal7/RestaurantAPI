using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public class AccountService : IAccountService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IPasswordHasher<User> _passwordHasher;

		public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
			_dbContext = dbContext;
			_passwordHasher = passwordHasher;
		}
        public void RegisterUser(RegisterUserDto dto)
		{
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
	}
}
