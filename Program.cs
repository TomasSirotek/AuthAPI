using AutoMapper;
using Data_Access.Data;
using Data_Access.Dtos;
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


// => DI Container 
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

app.MapGet("api/v1/characters", async (ICharacterRepository repository, IMapper mapper) =>
{
    var characters = await repository.GetAsync();
    return Results.Ok(mapper.Map<List<Character>>(characters));
});

app.MapGet("api/v1/characters/{id}", async (ICharacterRepository repository, IMapper mapper,string id) =>
{
    var character = await repository.GetByIdAsync(id);
    if (character == null)
        return Results.NotFound($"Character with Id: {id} was not found ");
    return Results.Ok(mapper.Map<Character>(character));
});


app.MapPost("api/v1/characters", async (ICharacterRepository repository, IMapper mapper,PostCharacterModel model) =>
{
    var character = mapper.Map<Character>(model);

    
    await repository.CreateAsync(character);
    await repository.SaveAsync();
    
    var readCharacter = mapper.Map<Character>(character);
    return Results.Created($"api/v1/characters/{character.Id}", readCharacter);

});



app.Run();