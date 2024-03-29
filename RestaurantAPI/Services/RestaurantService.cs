﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
	public class RestaurantService : IRestaurantService
	{
		private readonly RestaurantDbContext _dbcontext;
		private readonly IMapper _mapper;
		private readonly ILogger<RestaurantService> _logger;
		private readonly IAuthorizationService _authorizationService;
		private readonly IUserContextService _userContextService;

		public RestaurantService(RestaurantDbContext dbcontext, IMapper mapper, 
			ILogger<RestaurantService> logger, IAuthorizationService authorizationService,
			IUserContextService userContextService)
		{
			_dbcontext = dbcontext;
			_mapper = mapper;
			_logger = logger;
			_authorizationService = authorizationService;
			_userContextService = userContextService;
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

		public PagedResult<RestaurantDto> GetAll(RestaurantQuery query)
		{
			_logger.LogWarning($"Restaurant GET ALL action invoked");

			var baseQuery = _dbcontext
				.Restaurants
				.Include(r => r.Address)
				.Include(r => r.Dishes)
				.Where(r => query.SearchPhrase == null || (r.Name.ToLower().Contains(query.SearchPhrase.ToLower()) || r.Description.ToLower().Contains(query.SearchPhrase.ToLower())));

			var columnsSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>
			{
				[nameof(Restaurant.Name)] = r => r.Name,
				[nameof(Restaurant.Description)] = r => r.Description,
				[nameof(Restaurant.Category)] = r => r.Category
			};

			if (!string.IsNullOrEmpty(query.SortBy) && columnsSelectors.ContainsKey(query.SortBy))
			{
				var selectedColumn = columnsSelectors[query.SortBy];

				baseQuery = query.SortDirection == SortDirection.ASC
					? baseQuery.OrderBy(selectedColumn)
					: baseQuery.OrderByDescending(selectedColumn);
			}

			var restaurants = baseQuery
				.Skip(query.PageSize * (query.PageNumber - 1))
				.Take(query.PageSize)
				.ToList();

			var totalItemsCount = baseQuery.Count();

			var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

			var result = new PagedResult<RestaurantDto>(restaurantsDtos, totalItemsCount, query.PageSize, query.PageNumber);

			return result;
		}

		public int Create(CreateRestaurantDto dto)
		{
			_logger.LogWarning($"Restaurant CREATE action invoked");

			var restaurant = _mapper.Map<Restaurant>(dto);
			restaurant.CreatedById = _userContextService.GetUserId;

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

			var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

			if (!authorizationResult.Succeeded)
			{
				throw new ForbidException();
			}

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

			var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

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
