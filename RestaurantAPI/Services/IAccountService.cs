using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public interface IAccountService
	{
		void RegisterUser(RegisterUserDto dto);
		string GenerateJwt(LoginUserDto dto);
	}
}