using UserManagementAPI.BL.Services;

namespace UserManagementAPI.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtService _jwtService;

        private readonly List<PathString> _publicPaths = new()
        {
            "/api/auth/signup",
            "/api/auth/signin",
            "/api/dev"
        };

        public JwtMiddleware(RequestDelegate next, IJwtService jwtService)
        {
            _next = next;
            _jwtService = jwtService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            if (_publicPaths.Any(p => path.StartsWithSegments(p)))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Cookies["jwt_token"];
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Missing token.");
                return;
            }

            var result = _jwtService.ValidateToken(token);

            if (!result.IsSuccess)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Cookies.Delete("jwt_token");
                await context.Response.WriteAsync($"Unauthorized: {string.Join(", ", result.Errors)}");
                return;
            }

            context.User = result.Value;
            await _next(context);
        }
    }
}
