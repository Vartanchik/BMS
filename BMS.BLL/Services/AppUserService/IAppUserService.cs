using BMS.BLL.DTOs;
using System.Threading.Tasks;

namespace BMS.BLL.Services.AppUserService
{
    public interface IAppUserService
    {
        Task<AppUserDto> GetAsync(int usertId);
        Task<ServiceResult> CreateAsync(AppUserDto appUserDto, string password);
        Task<int> GetUserIdAsync(string email);
        Task<bool> CheckPasswordAsync(int userId, string password);
        string GenerateToken(int userId, string securityKey, string expireTime);
    }
}
