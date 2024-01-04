﻿using AutoMapper;
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
			var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == restaurantId) ?? throw new NotFoundException("Restaurant not found");
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
			var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == restaurantId) ?? throw new NotFoundException("Restaurant not found");

			var dishes = _context.Dishes.Where(d => d.RestaurantId == restaurantId);

			var dishesDtos = _mapper.Map<List<DishDto>>(dishes);

			return dishesDtos;
		}
	}
}
