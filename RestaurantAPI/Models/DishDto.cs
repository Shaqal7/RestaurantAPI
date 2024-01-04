using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
	public class DishDto
	{
		[Required]
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
	}
}
