using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RestaurantController : ControllerBase
	{
		private readonly RestaurantDbContext _dbContext;
		
		public RestaurantController(RestaurantDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Restaurant>> GetAll()
		{
			List<Restaurant> restaurants = _dbContext.Restaurants.ToList();
			return Ok(restaurants);
		}

		[HttpGet("{id}")]
		public ActionResult<Restaurant> Get([FromRoute] int id)
		{
			Restaurant restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);
			if (restaurant is null)
			{
				return NotFound();
			}

			return Ok(restaurant);
		}
	}
}
