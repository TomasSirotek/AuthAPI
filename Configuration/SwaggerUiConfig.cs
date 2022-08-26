using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace ProductAPI.Configuration {
    public static class SwaggerUiConfig {
    
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DevApi",
                    Version = "v1",
                    Description = "This is development API "
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,

                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }
        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductAPI v1");
            });
       
        }
    }
}