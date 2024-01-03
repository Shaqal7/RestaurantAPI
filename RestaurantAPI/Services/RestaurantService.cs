using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public class RestaurantService : IRestaurantService
	{
		private readonly RestaurantDbContext _dbcontext;
		private readonly IMapper _mapper;
		public RestaurantService(RestaurantDbContext dbcontext, IMapper mapper)
		{
			_dbcontext = dbcontext;
			_mapper = mapper;
		}
		public RestaurantDto GetById(int id)
		{
			var restaurant = _dbcontext
				.Restaurants
				.Include(r => r.Address)
				.Include(r => r.Dishes)
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null)
			{
				return null;
			}

			var result = _mapper.Map<RestaurantDto>(restaurant);
			return result;
		}

		public IEnumerable<RestaurantDto> GetAll()
		{
			var restaurants = _dbcontext
				.Restaurants
				.Include(r => r.Address)
				.Include(r => r.Dishes)
				.ToList();

			var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

			return restaurantsDtos;
		}

		public int Create(CreateRestaurantDto dto)
		{
			var restaurant = _mapper.Map<Restaurant>(dto);

			_dbcontext.Restaurants.Add(restaurant);
			_dbcontext.SaveChanges();

			return restaurant.Id;
		}

		public bool Delete(int id)
		{
			var restaurant = _dbcontext
				.Restaurants
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null) return false;

			_dbcontext.Restaurants.Remove(restaurant);
			_dbcontext.SaveChanges();

			return true;
		}

		public bool Update(UpdateRestaurantDto dto, int id)
		{
			var restaurant = _dbcontext
				.Restaurants
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null) return false;

			restaurant.Name = dto.Name;
			restaurant.Description = dto.Description;
			restaurant.HasDelivery = dto.HasDelivery;

			_dbcontext.SaveChanges();

			return true;
		}
	}
}
