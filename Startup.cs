// using System.Text;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.OpenApi.Models;
// using Newtonsoft.Json.Serialization;
// using ProductAPI.Domain.Profiles;
// using ProductAPI.Infrastructure.Data;
// using ProductAPI.Infrastructure.Repositories;
// using Swashbuckle.AspNetCore.Filters;
//
// namespace ProductAPI
// {
//     public class Startup
//     {
//         public Startup(IConfiguration configuration)
//         {
//             Configuration = configuration;
//         }
//
//         public IConfiguration Configuration { get; }
//
//         // This method gets called by the runtime. Use this method to add services to the container.
//         public void ConfigureServices(IServiceCollection services)
//         {
//             // TODO: Get here dependency service
//             
//             // Enable CORS // marked twice also in app. // needs to be changed 
//             services.AddCors(c =>
//             {
//                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
//             });
//             
//             
//             // JSON Serialization // Nuget package
//             // services.AddControllersWithViews().AddNewtonsoftJson(options =>
//             //         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
//             //     .AddNewtonsoftJson(options =>
//             //         options.SerializerSettings.ContractResolver = new DefaultContractResolver());
//             //
//             services.AddAutoMapper((typeof(ModelsProfiles)));
//             
//            //  services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
//
//             services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
//             
//         
//         }
//         // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//         public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
//         {
//             if (env.IsDevelopment()) {
//                 app.UseDeveloperExceptionPage();
//                 app.UseSwagger();
//                 //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductsAPI v1"));
//             }
//
//             app.UseCors((options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
//             
//             app.UseHttpsRedirection();
//
//             app.UseRouting();
//
//             app.UseAuthentication ();
//
//             app.UseAuthorization();
//
//             app.UseStaticFiles();
//
//             app.UseEndpoints(endpoints => {
//                 endpoints.MapControllers();
//             });
//         }
//     }
//
//
// }