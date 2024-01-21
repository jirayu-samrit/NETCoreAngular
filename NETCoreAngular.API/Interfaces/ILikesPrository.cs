using NETCoreAngular.API.DTOs;
using NETCoreAngular.API.Entities;
using NETCoreAngular.API.Heplers;

namespace NETCoreAngular.API.Interfaces;
public interface ILikesPrository
{
    Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
    Task<AppUser> GetUserWithLikes(int userId);
    Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
}