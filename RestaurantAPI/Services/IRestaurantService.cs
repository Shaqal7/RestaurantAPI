using RestaurantAPI.Models;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
	public interface IRestaurantService
	{
		int Create(CreateRestaurantDto dto, int userId);
		void Delete(int id, ClaimsPrincipal user);
		IEnumerable<RestaurantDto> GetAll();
		RestaurantDto GetById(int id);
		void Update(UpdateRestaurantDto dto, int id, ClaimsPrincipal user);
	}
}