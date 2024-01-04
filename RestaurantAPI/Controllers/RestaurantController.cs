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
			_restaurantService.Update(dto, id, User);

			return Ok();
		}

		[HttpDelete("{id}")]
		public ActionResult Delete([FromRoute] int id)
		{
			_restaurantService.Delete(id, User);

			return NoContent();
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Manager")]
		public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
		{
			var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

			int id = _restaurantService.Create(dto, userId);

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
