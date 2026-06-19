using _2380601597_TranMinhNhut_WebAPI.Models;

namespace _2380601597_TranMinhNhut_WebAPI.Services
{
    public interface IJwtTokenService
    {
        string CreateToken(AppUser user);
    }
}