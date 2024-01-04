using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
	public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
	{
		public RegisterUserDtoValidator(RestaurantDbContext dbContext)
		{
			RuleFor(x => x.Email).EmailAddress().NotEmpty();
			RuleFor(x => x.Password).MinimumLength(6).NotEmpty();
			RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);

			RuleFor(x =>  x.Email).Custom((value, context) =>
			{
				bool emailInUse = dbContext.Users.Any(u => u.Email == value);

				if (emailInUse)
				{
					context.AddFailure("Email", "That email address is taken");
				}
			});
		}
	}
}
