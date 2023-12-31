﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCoreAngular.API.Data;
using NETCoreAngular.API.DTOs;
using NETCoreAngular.API.Entities;
using NETCoreAngular.API.Interfaces;

namespace NETCoreAngular.API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserReposioty _userReposioty;
    private readonly IMapper _mapper;

    public UsersController(IUserReposioty userReposioty, IMapper mapper)
    {
        _userReposioty = userReposioty;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userReposioty.GetMembersAsync();
        return Ok(users);

    }

    [HttpGet("{username}")] // /api/users/2
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        return await _userReposioty.GetMemberByUsernameAsync(username);
    }

}
