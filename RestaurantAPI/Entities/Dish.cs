namespace RestaurantAPI.Entities
{
	public class Dish
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Price { get; set; }

		public int RestaurantId { get; set; }
		public virtual Restaurant Restaurant { get; set; }
	}
}