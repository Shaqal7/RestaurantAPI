using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class RestaurantController : ControllerBase
	{
		private readonly IRestaurantService _restaurantService;

		public RestaurantController(IRestaurantService restaurantService)
		{
			_restaurantService = restaurantService;
		}

		[HttpDelete("{id}")]
		public ActionResult Delete([FromRoute] int id)
		{
			bool isDeleted = _restaurantService.Delete(id);

			if (isDeleted)
			{
				return NoContent();
			}

			return NotFound();
		}

		[HttpPost]
		public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
		{
			if(ModelState.IsValid == false)
			{
				return BadRequest(ModelState);
			}

			int id = _restaurantService.Create(dto);

			return Created($"/api/restaurant/{id}", null);
		}
	

		[HttpGet]
		public ActionResult<IEnumerable<RestaurantDto>> GetAll()
		{
			var restaurantsDtos = _restaurantService.GetAll();

			return Ok(restaurantsDtos);
		}

		[HttpGet("{id}")]
		public ActionResult<RestaurantDto> Get([FromRoute] int id)
		{
			var restaurant = _restaurantService.GetById(id);

			if (restaurant is null)
			{
				return NotFound();
			}

			return Ok(restaurant);
		}
	}
}
