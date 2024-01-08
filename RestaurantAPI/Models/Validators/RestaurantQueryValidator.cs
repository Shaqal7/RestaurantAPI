using FluentValidation;

namespace RestaurantAPI.Models.Validators
{
	public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
	{
		private int[] allowedPageSizes = new[] { 5, 10, 15 };
		public RestaurantQueryValidator()
		{
			RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
			RuleFor(x => x.PageSize).Custom((value, context) =>
			{
				if (!allowedPageSizes.Contains(value))
				{
					context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
				}
			});
		}
	}
}
