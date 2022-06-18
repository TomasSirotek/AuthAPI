namespace ProductAPI.Configuration {
    public static class ApiConfig {
    
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // services.ConfigureProviderForContext<Context>(DetectDatabase(configuration));

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
        
        }

        public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowOrigin");
        
            app.MapControllers();
        
        }
    }
}
