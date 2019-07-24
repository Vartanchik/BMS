using System.Threading.Tasks;
using AutoMapper;
using BMS.BLL.DTOs;
using BMS.BLL.Services.AppUserService;
using BMS.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BMS.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountController(IAppUserService appUserService, IMapper mapper, IConfiguration configuration)
        {
            _appUserService = appUserService;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AppUserDto), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<AppUserDto>> Get([FromRoute] int id)
        {
            var user = await _appUserService.GetAsync(id);

            return user == null
                ? (ActionResult)NoContent()
                : Ok(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="appUserModel"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterModel appUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            var appUserDto = _mapper.Map<AppUserDto>(appUserModel);

            var result = await _appUserService.CreateAsync(appUserDto, appUserModel.Password);

            return result.Succeeded
                ? Ok(new ResponseModel(200, "Completed.", "User created."))
                : (ActionResult)BadRequest(new ResponseModel(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Get jwt access token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [AllowAnonymous]
        public async Task<ActionResult> GetAccess([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            // Checking if user exist
            var userId = await _appUserService.GetUserIdAsync(model.Email);

            if (userId < 1)
            {
                return BadRequest(new ResponseModel(400, "Failed", "User not found."));
            }

            // Checking password
            var passwordIsRight = await _appUserService.CheckPasswordAsync(userId, model.Password);

            if (!passwordIsRight)
            {
                return BadRequest(new ResponseModel(400, "Failed", "Wrong password."));
            }

            // Generate jwt token
            var token = _appUserService.GenerateToken(userId, _configuration["Jwt:Key"], _configuration["Jwt:ExpireTime"]);

            return Ok(new { Token = token });
        }
    }
}