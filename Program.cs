using AuthAPI.Configuration;
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

builder.Services.AddEmailConfiguration(builder.Configuration);


var app = builder.Build();
#endregion

#region Configure Pipeline

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.UseAuthConfiguration();

app.Run();

#endregion