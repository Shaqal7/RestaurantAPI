namespace RestaurantAPI.Middleware
{
	using Microsoft.AspNetCore.Http;
	using System.Threading.Tasks;

	public class ErrorHandlingMiddleware : IMiddleware
	{
		private ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
		{
			_logger = logger;
		}
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next.Invoke(context);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);

				context.Response.StatusCode = 500;
				await context.Response.WriteAsync("Something went wrong");
			}
		}
	}
}
