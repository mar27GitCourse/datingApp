using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _TokenService;

        public AccountController(DataContext context, ITokenService service)
        {
            _TokenService = service;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.UserName)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserName = user.UserName,
                Token = _TokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.User
            .SingleOrDefaultAsync(_ => _.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("Invalid userName");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = _TokenService.CreateToken(user)
            };

        }

        private async Task<bool> UserExist(string userName)
        {
            return await _context.User.AnyAsync(_ => _.UserName == userName.ToLower());
        }
    }
}