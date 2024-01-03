using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public class RestaurantService : IRestaurantService
	{
		private readonly RestaurantDbContext _dbcontext;
		private readonly IMapper _mapper;
		private readonly ILogger<RestaurantService> _logger;
		public RestaurantService(RestaurantDbContext dbcontext, IMapper mapper, ILogger<RestaurantService> logger)
		{
			_dbcontext = dbcontext;
			_mapper = mapper;
			_logger = logger;
		}
		public RestaurantDto GetById(int id)
		{
			var restaurant = _dbcontext
				.Restaurants
				.Include(r => r.Address)
				.Include(r => r.Dishes)
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");

			var result = _mapper.Map<RestaurantDto>(restaurant);
			return result;
		}

		public IEnumerable<RestaurantDto> GetAll()
		{
			_logger.LogWarning($"Restaurant GET ALL action invoked");

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
			_logger.LogWarning($"Restaurant CREATE action invoked");

			var restaurant = _mapper.Map<Restaurant>(dto);

			_dbcontext.Restaurants.Add(restaurant);
			_dbcontext.SaveChanges();

			return restaurant.Id;
		}

		public void Delete(int id)
		{
			_logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");

			var restaurant = _dbcontext
				.Restaurants
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");

			_dbcontext.Restaurants.Remove(restaurant);
			_dbcontext.SaveChanges();
		}

		public void Update(UpdateRestaurantDto dto, int id)
		{
			_logger.LogWarning($"Restaurant with id: {id} UPDATE action invoked");

			var restaurant = _dbcontext
				.Restaurants
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");

			restaurant.Name = dto.Name;
			restaurant.Description = dto.Description;
			restaurant.HasDelivery = dto.HasDelivery;

			_dbcontext.SaveChanges();
		}
	}
}
