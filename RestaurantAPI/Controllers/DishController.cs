﻿using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [ApiController]
	[Route("api/restaurant/{restaurantId}/[controller]")]
	public class DishController : ControllerBase
	{
		private readonly IDishService _dishService;

		public DishController(IDishService dishService)
		{
			_dishService = dishService;
		}

		[HttpPost]
		public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
		{
			var newDishId = _dishService.Create(restaurantId, dto);

			return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
		}

		[HttpGet("{dishId}")]
		public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
		{
			var dish = _dishService.GetById(restaurantId, dishId);

			return Ok(dish);
		}

		[HttpGet]
		public ActionResult<List<DishDto>> GetAll([FromRoute] int restaurantId)
		{
			var dishes = _dishService.GetAll(restaurantId);

			return Ok(dishes);
		}
	}
}
