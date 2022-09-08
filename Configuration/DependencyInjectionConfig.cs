using AuthAPI.Configuration.Token;
using AuthAPI.Engines;
using AuthAPI.Engines.Cryptography;
using AuthAPI.ExternalServices;
using AuthAPI.Identity.BindingModels;
using AuthAPI.Identity.BindingModels.Authentication;
using AuthAPI.Infrastructure.Data;
using AuthAPI.Infrastructure.Repositories;
using AuthAPI.Infrastructure.Repositories.Interfaces;
using AuthAPI.Services;
using AuthAPI.Services.Interfaces;
using AuthAPI.Validations;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace AuthAPI.Configuration {
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