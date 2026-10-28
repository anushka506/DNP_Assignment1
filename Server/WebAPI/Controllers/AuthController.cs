using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public ActionResult<UserDto> Login(LoginRequest request)
    {
        User? user = userRepository.GetMany()
            .FirstOrDefault(u => u.UserName == request.UserName);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        if (user.Password != request.Password)
        {
            return Unauthorized("Invalid username or password.");
        }

        UserDto userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };

        return Ok(userDto);
    }
}