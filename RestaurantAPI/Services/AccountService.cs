using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public class AccountService : IAccountService
	{
		private readonly RestaurantDbContext _dbContext;

        public AccountService(RestaurantDbContext dbContext)
        {
			_dbContext = dbContext;
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

			_dbContext.Users.Add(newUser);
			_dbContext.SaveChanges();
		}
	}
}
