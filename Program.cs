using Microsoft.AspNetCore.Identity;
using ProductAPI.Configuration;
using ProductAPI.Helpers;
using ProductAPI.Identity;
using ProductAPI.Identity.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger());

#region Configure Services

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.RegisterServices();

// Add identity types
builder.Services.AddIdentity<AppUser, UserRole>()
    .AddDefaultTokenProviders();

// Identity Services 
builder.Services.AddTransient<IUserStore<AppUser>, AppUserStore>();
 builder.Services.AddTransient<IRoleStore<UserRole>, AppRoleStore>();

var app = builder.Build();
#endregion

#region Configure Pipeline

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

// custom jwt auth middleware
//app.UseMiddleware<JwtMiddleware>();

app.UseAuthConfiguration();

app.Run();

#endregion