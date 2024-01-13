
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCoreAngular.API.Data;
using NETCoreAngular.API.DTOs;
using NETCoreAngular.API.Entities;
using NETCoreAngular.API.Interfaces;

namespace NETCoreAngular.API.Controllers;
public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
    {
        _context = context;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")] // POST: /api/account/register?username=sam&password=password
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username))
        {
            return BadRequest("Username has already exist.");
        }

        var user = _mapper.Map<AppUser>(registerDto);

        using var hmac = new HMACSHA512();

        user.UserName = registerDto.Username.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PasswordSalt = hmac.Key;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var loginUser = new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };

        return loginUser;
    }


    [HttpPost("login")] // POST: /api/account/login?username=sam&password=password
    public async Task<ActionResult<UserDto>> login(LoginDto loginDto)
    {
        var user = await _context.Users
                    .Include(p => p.Photos)
                    .SingleOrDefaultAsync(u => u.UserName.ToLower() == loginDto.Username.ToLower());
        if (user == null) return Unauthorized("User does not exist.");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        if (computeHash.Length != user.PasswordHash.Length) return Unauthorized("Invaid Password");

        for (int i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("Invaid Password");
        }

        var loginUser = new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };

        return loginUser;
    }

    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(a => a.UserName.ToLower() == username.ToLower());
    }
}
