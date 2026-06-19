using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using _2380601597_TranMinhNhut_WebAPI.Data;

namespace _2380601597_TranMinhNhut_WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.AppUsers
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.Role
                })
                .OrderByDescending(u => u.Id)
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            return Ok(new
            {
                id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                fullName = User.FindFirst(ClaimTypes.Name)?.Value,
                email = User.FindFirst(ClaimTypes.Email)?.Value,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }
    }
}