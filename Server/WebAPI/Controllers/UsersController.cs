using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    // CREATE
    [HttpPost]
    public async Task<ActionResult<User>> CreateAsync(User user)
    {
        User createdUser = await userRepository.AddAsync(user);

        return CreatedAtAction(
            nameof(GetSingleAsync),
            new { id = createdUser.Id },
            createdUser);
    }

    // GET SINGLE
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetSingleAsync(int id)
    {
        User user = await userRepository.GetSingleAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    // GET MANY
    // Example: GET /api/users?userName=anna
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetMany(
        [FromQuery] string? userName)
    {
        IQueryable<User> users = userRepository.GetMany();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            users = users.Where(user =>
                user.UserName.Contains(userName));
        }

        return Ok(users.ToList());
    }

    // UPDATE
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        User user)
    {
        if (id != user.Id)
        {
            return BadRequest("ID in URL must match user ID.");
        }

        await userRepository.UpdateAsync(user);

        return NoContent();
    }

    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await userRepository.DeleteAsync(id);

        return NoContent();
    }
}