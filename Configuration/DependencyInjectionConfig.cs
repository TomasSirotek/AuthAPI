using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories;
using ProductAPI.Services;

namespace ProductAPI.Configuration {
    public static class DependencyInjectionConfig {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<SqlServerConnection>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IStartupFilter, DatabaseExtension>();
            services.AddTransient<DbCustomLogger<DatabaseExtension>>();
        }
    }
}