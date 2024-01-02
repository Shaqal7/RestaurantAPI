namespace RestaurantAPI.Entities
{
	public class RestaurantSeeder
	{
		private readonly RestaurantDbContext _dbContext;
		public RestaurantSeeder(RestaurantDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void Seed()
		{
			if (_dbContext.Database.CanConnect())
			{
				if (!_dbContext.Restaurants.Any())
				{
					var restaurants = GetRestaurants();
					_dbContext.Restaurants.AddRange(restaurants);
					_dbContext.SaveChanges();
				}
			}
		}

		private IEnumerable<Restaurant> GetRestaurants()
		{
			var restaurants = new List<Restaurant>()
			{
				new Restaurant()
				{
					Name = "KFC",
					Description = "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken. It is the world's second-largest restaurant chain (as measured by sales) after McDonald's, with 22,621 locations globally in 150 countries as of December 2019. The chain is a subsidiary of Yum! Brands, a restaurant company that also owns the Pizza Hut, Taco Bell, and WingStreet chains.",
					Category = "Fast Food",
					HasDelivery = true,
					ContactEmail = "contact@kfc.com",
					ContactNumber = "1-800-225-5532",
					Address = new Address()
					{
						City = "Louisville",
						Street = "1441 Gardiner Lane",
						PostalCode = "40213",
					},
					Dishes = new List<Dish>()
					{
						new Dish()
						{
							Name = "Chicken wings",
							Price = 7.99M
						},
						new Dish()
						{
							Name = "Chicken poppers",
							Price = 5.99M
						}
					}
				},
				new Restaurant()
				{
					Name = "McDonald's",
					Description = "McDonald's Corporation is an American fast food company, founded in 1940 as a restaurant operated by Richard and Maurice McDonald, in San Bernardino, California, United States. They rechristened their business as a hamburger stand, and later turned the company into a franchise, with the Golden Arches logo being introduced in 1953 at a location in Phoenix, Arizona. In 1955, Ray Kroc, a businessman, joined the company as a franchise agent and proceeded to purchase the chain from the McDonald brothers. McDonald's had its previous headquarters in Oak Brook, Illinois, but moved its global headquarters to Chicago in June 2018.",
					Category = "Fast Food",
					HasDelivery = true,
					ContactEmail = "contact@mcdonalds.com",
					ContactNumber = "1-800-244-6227",
					Address = new Address()
					{
						City = "Chicago",
						Street = "2111 McDonald's Dr",
						PostalCode = "60523",
					},
					Dishes = new List<Dish>()
					{
						new Dish()
						{
							Name = "Big Mac",
							Price = 4.99M
						},
						new Dish()
						{
							Name = "Cheeseburger",
							Price = 2.99M
						}
					}
				}
			};

			return restaurants;
		}
	}
}
