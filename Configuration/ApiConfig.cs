using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using ProductAPI.Helpers;
using ProductAPI.Identity;
using ProductAPI.Identity.Models;

namespace ProductAPI.Configuration {
    public static class ApiConfig {
    
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            
            services.AddFluentValidationAutoValidation(fv => fv.DisableDataAnnotationsValidation = false).AddFluentValidationClientsideAdapters();
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

            app.UseRouting();

            app.UseCors("AllowOrigin");

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.MapControllers();
        
        }
    }
}
