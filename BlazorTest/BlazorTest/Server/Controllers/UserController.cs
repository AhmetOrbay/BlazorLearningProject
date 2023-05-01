using BlazorTest.Server.Extensions.Insfrastruce;
using BlazorTest.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTest.Server.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ServiceResponse<UserLoginResponseDTO>> LoginUser(UserLoginRequestDTO userLogin)
        {
            return new ServiceResponse<UserLoginResponseDTO>
            {
                Data = await _userService.Login(userLogin)
            };
        }

        [HttpGet("Users")]
        public async Task<ServiceResponse<List<UserDto>>> GetUsers()
        {
            ServiceResponse<List<UserDto>> result = new ();
            try
            {
                var user = await _userService.GetUsers();
                result.Data = user;
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
            return result;


        }

        [HttpGet("UserSingle/{Id}")]
        public async Task<ServiceResponse<UserDto>> GetUserById(Guid Id)
        {
            ServiceResponse<UserDto> result = new();
            try
            {
                var user = await _userService.GetById(Id);
                result.Data = user;
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
            return result;


        }

        [HttpPost("addUser")]
        public async Task<ServiceResponse<UserDto>> UserInsert(UserDto user)
        {
            ServiceResponse<UserDto> result = new();
            try
            {
                user.CreateTime = DateTime.UtcNow;
                var userResult = await _userService.CreateUser(user);
                result.Data = userResult;
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
            return result;


        }
        [HttpPost("updateUser")]
        public async Task<ServiceResponse<UserDto>> UserUpdate(UserDto user)
        {
            ServiceResponse<UserDto> result = new();
            try
            {
                var userResult = await _userService.UpdateUser(user);
                result.Data = userResult;
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
            return result;
        }

        [HttpPost("deleteUser")]
        public async Task<ServiceResponse<bool>> UserDelete([FromBody] Guid Id)
        {
            ServiceResponse<bool> result = new();
            try
            {
                var userResult = await _userService.DeleteUserById(Id);
                result.Data = userResult;
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
            return result;
        }
    }
}
