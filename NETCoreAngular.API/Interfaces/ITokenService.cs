using NETCoreAngular.API.Entities;

namespace NETCoreAngular.API.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}