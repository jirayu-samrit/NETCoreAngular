using NETCoreAngular.API.DTOs;
using NETCoreAngular.API.Entities;

namespace NETCoreAngular.API.Interfaces;
public interface IUserReposioty
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<IEnumerable<MemberDto>> GetMembersAsync();
    Task<MemberDto> GetMemberByUsernameAsync(string username);

}