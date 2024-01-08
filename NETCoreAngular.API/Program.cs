using Microsoft.EntityFrameworkCore;
using NETCoreAngular.API.Data;
using NETCoreAngular.API.Extensions;
using NETCoreAngular.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
app.UseCors(cors =>
{
    cors
    .WithOrigins("https://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch(Exception ex)
{   
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex,"An error occurred during migration");
}

app.Run();
