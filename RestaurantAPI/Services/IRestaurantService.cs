using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public interface IRestaurantService
	{
		int Create(CreateRestaurantDto dto);
		bool Delete(int id);
		IEnumerable<RestaurantDto> GetAll();
		RestaurantDto GetById(int id);
	}
}