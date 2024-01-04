using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
	public class DishService : IDishService
	{
		private readonly RestaurantDbContext _context;
		private readonly IMapper _mapper;
		public DishService(RestaurantDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public int Create(int restaurantId, CreateDishDto dto)
		{
			Restaurant? restaurant = GetRestaurantById(restaurantId);
			var dishEntity = _mapper.Map<Dish>(dto);
			dishEntity.RestaurantId = restaurantId;

			_context.Dishes.Add(dishEntity);
			_context.SaveChanges();

			return dishEntity.Id;
		}

		public DishDto GetById(int restaurantId, int dishId)
		{
			var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId && d.RestaurantId == restaurantId) ?? throw new NotFoundException("Dish not found");

			var dishDto = _mapper.Map<DishDto>(dish);

			return dishDto;
		}

		public List<DishDto> GetAll(int restaurantId)
		{
			Restaurant? restaurant = GetRestaurantById(restaurantId);

			var dishesDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);

			return dishesDtos;
		}

		public void RemoveAll(int restaurantId)
		{
			Restaurant? restaurant = GetRestaurantById(restaurantId);

			_context.Dishes.RemoveRange(restaurant.Dishes);
			_context.SaveChanges();
		}

		public void RemoveById(int restaurantId, int dishId)
		{
			Restaurant? restaurant = GetRestaurantById(restaurantId);

			var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId) ?? throw new NotFoundException("Dish not found");

			_context.Dishes.Remove(dish);
			_context.SaveChanges();
		}

		private Restaurant GetRestaurantById(int restaurantId)
		{
			var restaurant = _context
				.Restaurants
				.Include(r => r.Dishes)
				.FirstOrDefault(r => r.Id == restaurantId) ?? throw new NotFoundException("Restaurant not found");

			return restaurant;
		}
	}
}
