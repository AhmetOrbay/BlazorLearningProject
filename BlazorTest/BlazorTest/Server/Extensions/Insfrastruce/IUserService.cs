using BlazorTest.Shared.Dto;

namespace BlazorTest.Server.Extensions.Insfrastruce
{
    public interface IUserService
    {
        public Task<UserDto> GetById(Guid Id);
        public Task<List<UserDto>> GetUsers();
        public Task<UserDto> CreateUser(UserDto user);
        public Task<UserDto> UpdateUser(UserDto user);
        public Task<bool> DeleteUserById(Guid Id);
        public Task<UserLoginResponseDTO> Login(UserLoginRequestDTO user);

    }
}
