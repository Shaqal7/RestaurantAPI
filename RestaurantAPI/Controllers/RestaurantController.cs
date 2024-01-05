using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class RestaurantController : ControllerBase
	{
		private readonly IRestaurantService _restaurantService;

		public RestaurantController(IRestaurantService restaurantService)
		{
			_restaurantService = restaurantService;
		}

		[HttpPut("{id}")]
		public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
		{
			_restaurantService.Update(dto, id);

			return Ok();
		}

		[HttpDelete("{id}")]
		public ActionResult Delete([FromRoute] int id)
		{
			_restaurantService.Delete(id);

			return NoContent();
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Manager")]
		public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
		{
			int id = _restaurantService.Create(dto);

			return Created($"/api/restaurant/{id}", null);
		}
	

		[HttpGet]
		[Authorize(Policy = "Atleast18")]
		public ActionResult<IEnumerable<RestaurantDto>> GetAll()
		{
			var restaurantsDtos = _restaurantService.GetAll();

			return Ok(restaurantsDtos);
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		public ActionResult<RestaurantDto> Get([FromRoute] int id)
		{
			var restaurant = _restaurantService.GetById(id);

			return Ok(restaurant);
		}
	}
}
