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
            // Category DI
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ICategoryService, CategoryService>();
        }
    }
}