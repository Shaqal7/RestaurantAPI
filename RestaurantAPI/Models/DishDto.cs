using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
	public class DishDto
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
	}
}
