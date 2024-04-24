using System.Security.Claims;
using WindSync.BLL.Services.Auth;

namespace WindSync.PL.Middleware;

public class UserIdMiddleware
{
    private readonly RequestDelegate _next;

    public UserIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var username = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        if (!string.IsNullOrEmpty(username))
        {
            var authService = context.RequestServices.GetService(typeof(IAuthService)) as IAuthService;
            var user = await authService.GetUserByUsernameAsync(username);
            if (user is not null)
            {
                context.Items["UserId"] = user.Id;
            }
        }

        await _next(context);
    }
}
