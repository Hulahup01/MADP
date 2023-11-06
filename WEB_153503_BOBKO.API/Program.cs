using Microsoft.EntityFrameworkCore;

using WEB_153503_BOBKO.API.Data;
using WEB_153503_BOBKO.API.Services.GameGenreService;
using WEB_153503_BOBKO.API.Services.GameServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IGameGenreService, GameGenreService>();
builder.Services.AddScoped<IGameService, GameService>();


builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default")));


var app = builder.Build();

//await DbInitializer.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
