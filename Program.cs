using AutoMapper;
using Data_Access.Data;
using Data_Access.Models;
using Data_Access.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConBuilder = new SqlConnectionStringBuilder();
sqlConBuilder.ConnectionString = builder.Configuration.GetConnectionString("SqlConnector");
sqlConBuilder.UserID = builder.Configuration["UserId"];
sqlConBuilder.Password = builder.Configuration["Password"];

// registering the db context to be presented as context in repositories 
builder.Services.AddDbContext<Context>(o => o.UseSqlServer(sqlConBuilder.ConnectionString));


// => DI Container needs to be move to seperate file later on !
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
// register automapper for DI 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseAuthorization();

app.Map("api/v1/characters", async (ICharacterRepository repository, IMapper mapper) =>
{
    var characters = await repository.GetAsync();
    return Results.Ok(mapper.Map<List<Character>>(characters));
});

app.Run();