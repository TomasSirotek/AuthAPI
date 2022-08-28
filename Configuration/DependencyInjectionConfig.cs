using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Configuration.Token;
using ProductAPI.Engines;
using ProductAPI.Engines.Cryptography;
using ProductAPI.ExternalServices;
using ProductAPI.Identity.BindingModels;
using ProductAPI.Identity.BindingModels.Authentication;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories;
using ProductAPI.Infrastructure.Repositories.Interfaces;
using ProductAPI.Services;
using ProductAPI.Services.Interfaces;
using ProductAPI.Validations;

namespace ProductAPI.Configuration {
    public static class DependencyInjectionConfig {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Server DI
            services.AddScoped<SqlServerConnection>();
            services.AddTransient<IStartupFilter, DatabaseExtension>();
            services.AddTransient<DbCustomLogger<DatabaseExtension>>();
            // User DI
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            // User DI
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IRoleService, RoleService>();
            // Token DI
            services.AddScoped<IJwtToken, JwtToken>();
            services.AddScoped<IVerifyJwtToken, VerifyJwtToken>();
            // Crypto DI
            services.AddScoped<ICryptoEngine, CryptoEngine>();
            // Date Converter
            services.AddScoped<IUnixStampDateConverter, UnixStampDateConverter>();
            // Token validation
            services.AddSingleton<TokenValidationParameters>();
            // Email DI
            services.AddTransient<IEmailService, EmailService>();
            // Validators : User
            services.AddScoped<IValidator<UserPostModel>, PostUserValidation>();
            services.AddScoped<IValidator<UserPutModel>, PutUserValidation>();
            // Validators : Role
            services.AddScoped<IValidator<RolePostModel>, RolePostValidation>();
            services.AddScoped<IValidator<RolePutModel>, RolePutValidation>();
            // Validators : Register
            services.AddScoped<IValidator<RegisterPostModel>, RegisterValidator>();
        }
    }
}