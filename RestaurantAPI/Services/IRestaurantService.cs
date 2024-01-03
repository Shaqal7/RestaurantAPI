using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public interface IRestaurantService
	{
		int Create(CreateRestaurantDto dto);
		void Delete(int id);
		IEnumerable<RestaurantDto> GetAll();
		RestaurantDto GetById(int id);
		void Update(UpdateRestaurantDto dto, int id);
	}
}