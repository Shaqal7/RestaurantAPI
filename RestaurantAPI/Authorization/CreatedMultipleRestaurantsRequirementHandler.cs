using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
	public class CreatedMultipleRestaurantsRequirementHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
	{
		private readonly RestaurantDbContext _dbContext;
		public CreatedMultipleRestaurantsRequirementHandler(RestaurantDbContext context)
		{
			_dbContext = context;
		}
		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
		{
			var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

			var createdRestaurantsCount = _dbContext
				.Restaurants
				.Count(r => r.CreatedById == userId);

			if (createdRestaurantsCount >= requirement.MinimumRestaurantsCreated)
			{
				context.Succeed(requirement);
			}

			await Task.CompletedTask;
		}
	}
}
