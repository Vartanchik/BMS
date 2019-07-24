using AutoMapper;
using BMS.BLL.DTOs;
using BMS.DAL.Entities;
using BMS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.AppUserService
{
    public class AppUserService : IAppUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AppUserService> _logger;

        public AppUserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AppUserService> logger)
        {
            _uow = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AppUserDto> GetAsync(int userId)
        {
            try
            {
                var appUser = await _uow.AppUsers.GetByIdAsync(userId);

                return _mapper.Map<AppUserDto>(appUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ToString()} - error message:{ex.Message}");

                return null;
            }
        }

        public async Task<ServiceResult> CreateAsync(AppUserDto appUserDto, string password)
        {
            try
            {
                // Checking if user already exist
                var appUserExist = await _uow.AppUsers.AsQueryable()
                                                      .AnyAsync(u => u.Email == appUserDto.Email &&
                                                                     u.UserName == appUserDto.Name);

                if (appUserExist) return new ServiceResult($"User with name: {appUserDto.Name} or email: {appUserDto.Email} - already exist.");

                // User creating
                var appUser = _mapper.Map<AppUser>(appUserDto);

                var result = await _uow.AppUsers.CreateAsync(appUser, password);

                return new ServiceResult { Succeeded = result.Succeeded };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ToString()} - error message:{ex.Message}");

                return new ServiceResult(ex.Message);
            }
        }

        public async Task<bool> CheckPasswordAsync(int userId, string password)
        {
            return await _uow.AppUsers.CheckPasswordAsync(userId, password);
        }

        public async Task<int> GetUserIdAsync(string email)
        {
            return await _uow.AppUsers.AsQueryable()
                                        .Where(u => u.Email == email)
                                        .Select(u => u.Id)
                                        .FirstOrDefaultAsync();
        }


        public string GenerateToken(int userId, string securityKey, string expireTime)
        {
            try
            {
                // Create symmetric security key
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

                // Create signin credentials
                var signinCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim("userId", userId.ToString())
                };

                // Create jwt token
                var token = new JwtSecurityToken(
                        expires: DateTime.UtcNow.AddDays(Convert.ToInt32(expireTime)),
                        signingCredentials: signinCredentials,
                        claims: claims
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ToString()} - error message:{ex.Message}");

                return null;
            }
        }
    }
}
