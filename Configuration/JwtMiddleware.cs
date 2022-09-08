using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProductAPI.Configuration.Token;
using ProductAPI.Helpers;
using ProductAPI.Identity.Models;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Configuration; 

// Currently not working 
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;
 

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context,IUserService userService, IJwtToken jwtToken)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtToken.ValidateJwtToken(token);
        if (userId != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = userService.GetUserByIdAsync(userId);
        }

        await _next(context);
    }
}