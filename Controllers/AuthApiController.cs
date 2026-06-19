using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2380601597_TranMinhNhut_WebAPI.Data;
using _2380601597_TranMinhNhut_WebAPI.DTOs;
using _2380601597_TranMinhNhut_WebAPI.Models;
using _2380601597_TranMinhNhut_WebAPI.Services;

namespace _2380601597_TranMinhNhut_WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly PasswordHasher<AppUser> _passwordHasher;

        public AuthApiController(
            ApplicationDbContext context,
            IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = new PasswordHasher<AppUser>();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Vui lòng nhập đầy đủ thông tin" });
            }

            var emailExists = await _context.AppUsers
                .AnyAsync(u => u.Email == request.Email);

            if (emailExists)
            {
                return BadRequest(new { message = "Email đã tồn tại" });
            }

            var role = request.Role.Trim();

            if (role != "Admin" && role != "User")
            {
                role = "User";
            }

            var user = new AppUser
            {
                FullName = request.FullName,
                Email = request.Email,
                Role = role
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đăng ký thành công",
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.Role
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
            }

            var token = _jwtTokenService.CreateToken(user);

            return Ok(new
            {
                message = "Đăng nhập thành công",
                token,
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.Role
                }
            });
        }
    }
}