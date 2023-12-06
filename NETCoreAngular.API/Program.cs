using Microsoft.EntityFrameworkCore;
using NETCoreAngular.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(builder =>
{
    builder
    .WithOrigins("https://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod();
});

app.MapControllers();

app.Run();
