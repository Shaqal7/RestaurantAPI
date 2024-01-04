using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
	public class RestaurantService : IRestaurantService
	{
		private readonly RestaurantDbContext _dbcontext;
		private readonly IMapper _mapper;
		private readonly ILogger<RestaurantService> _logger;
		private readonly IAuthorizationService _authorizationService;

		public RestaurantService(RestaurantDbContext dbcontext, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService)
		{
			_dbcontext = dbcontext;
			_mapper = mapper;
			_logger = logger;
			_authorizationService = authorizationService;
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

		public int Create(CreateRestaurantDto dto, int userId)
		{
			_logger.LogWarning($"Restaurant CREATE action invoked");

			var restaurant = _mapper.Map<Restaurant>(dto);
			restaurant.CreatedById = userId;

			_dbcontext.Restaurants.Add(restaurant);
			_dbcontext.SaveChanges();

			return restaurant.Id;
		}

		public void Delete(int id, ClaimsPrincipal user)
		{
			_logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");

			var restaurant = _dbcontext
				.Restaurants
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");

			var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

			if (!authorizationResult.Succeeded)
			{
				throw new ForbidException();
			}

			_dbcontext.Restaurants.Remove(restaurant);
			_dbcontext.SaveChanges();
		}

		public void Update(UpdateRestaurantDto dto, int id, ClaimsPrincipal user)
		{

			_logger.LogWarning($"Restaurant with id: {id} UPDATE action invoked");

			var restaurant = _dbcontext
				.Restaurants
				.FirstOrDefault(r => r.Id == id);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");

			var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

			if (!authorizationResult.Succeeded)
			{
				throw new ForbidException();
			}

			restaurant.Name = dto.Name;
			restaurant.Description = dto.Description;
			restaurant.HasDelivery = dto.HasDelivery;

			_dbcontext.SaveChanges();
		}
	}
}
