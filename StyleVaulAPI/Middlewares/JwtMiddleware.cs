using StyleVaulAPI.Interfaces.Services;

namespace StyleVaulAPI.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUsersService usersService)
        {
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()
                ?.Split(" ")
                .Last();
            var userId = usersService.ValidateJwtToken(token);
            if (userId != null)
            {
                context.Items["User"] = await usersService.GetByIdAsync(userId.Value);
            }

            await _next(context);
        }
    }
}