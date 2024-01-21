using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using NETCoreAngular.API.DTOs;
using NETCoreAngular.API.Entities;
using NETCoreAngular.API.Extensions;
using NETCoreAngular.API.Heplers;
using NETCoreAngular.API.Interfaces;

namespace NETCoreAngular.API.Controllers;
public class LikesController : BaseApiController
{
    private readonly IUserReposioty _userReposioty;
    private readonly ILikesPrository _likesPrository;

    public LikesController(IUserReposioty userReposioty, ILikesPrository likesPrository)
    {
        _userReposioty = userReposioty;
        _likesPrository = likesPrository;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
        var sourceUserId = User.GetUserId();
        var sourceUser = await _likesPrository.GetUserWithLikes(sourceUserId);
        var likedUser = await _userReposioty.GetUserByUsernameAsync(username);

        if (likedUser == null) return NotFound();
        if (sourceUser.UserName == username) return BadRequest("You cannot like yourself!");

        var userLike = await _likesPrository.GetUserLike(sourceUserId, likedUser.Id);
        if (userLike != null) return BadRequest("You already like this user");

        userLike = new UserLike
        {
            SourceUserId = sourceUserId,
            TargetUserId = likedUser.Id
        };

        sourceUser.LikedUsers.Add(userLike);

        if (await _userReposioty.SaveAllAsync()) return Ok();

        return BadRequest("Failed to like user");

    }

    [HttpGet]
    public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();

        var users = await _likesPrository.GetUserLikes(likesParams);

        Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

        return Ok(users);
    }
}