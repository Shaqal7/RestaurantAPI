using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Controllers
{
	public class HomeController : ControllerBase
	{
		[HttpGet("/")]
		public ActionResult Get()
		{
			return Ok("Hello from Restaurant API");
		}
	}
}
