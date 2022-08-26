using Microsoft.IdentityModel.Tokens;
using ProductAPI.Configuration.Token;
using ProductAPI.Engines;
using ProductAPI.Engines.Cryptography;
using ProductAPI.ExternalServices;
using ProductAPI.Identity;
using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories;
using ProductAPI.Infrastructure.Repositories.Interfaces;
using ProductAPI.Services;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Configuration {
    public static class DependencyInjectionConfig {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Server DI
            services.AddScoped<SqlServerConnection>();
            services.AddTransient<IStartupFilter, DatabaseExtension>();
            services.AddTransient<DbCustomLogger<DatabaseExtension>>();
            // Product DI
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();
            // User DI
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            // User DI
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IRoleService, RoleService>();
            // Token DI
            services.AddScoped<IJwtToken, JwtToken>();
            // Crypto DI
            services.AddScoped<ICryptoEngine, CryptoEngine>();
            
            services.AddScoped<IUnixStampDateConverter, UnixStampDateConverter>();
            // Token validation
            services.AddSingleton<TokenValidationParameters>();
            // Email DI
            services.AddTransient<IEmailService, EmailService>();

        }
    }
}