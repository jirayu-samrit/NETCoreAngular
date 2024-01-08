using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NETCoreAngular.API.Data;
using NETCoreAngular.API.Interfaces;
using NETCoreAngular.API.Services;

namespace NETCoreAngular.API.Extensions;
public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services
    , IConfiguration config)
    {
        services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserReposioty, UserRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
