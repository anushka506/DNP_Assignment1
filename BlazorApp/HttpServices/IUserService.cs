using ApiContracts;

namespace BlazorApp.HttpServices;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task UpdateUserAsync(int id, UpdateUserDto request);
}