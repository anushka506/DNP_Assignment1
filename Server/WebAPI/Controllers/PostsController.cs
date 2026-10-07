using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public PostsController(
        IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    // CREATE
    [HttpPost]
    public async Task<ActionResult<Post>> CreateAsync(Post post)
    {
        Post createdPost = await postRepository.AddAsync(post);

        return Created(
            $"api/posts/{createdPost.Id}",
            createdPost);
    }

    // GET SINGLE
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Post>> GetSingleAsync(int id)
    {
        Post post = await postRepository.GetSingleAsync(id);

        return Ok(post);
    }

    // GET MANY
    // Examples:
    // GET /api/posts?title=hello
    // GET /api/posts?userId=1
    // GET /api/posts?userName=anna
    [HttpGet]
    public ActionResult<IEnumerable<Post>> GetMany(
        [FromQuery] string? title,
        [FromQuery] int? userId,
        [FromQuery] string? userName)
    {
        IQueryable<Post> posts = postRepository.GetMany();

        // Filter by title
        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(post =>
                post.Title.Contains(title));
        }

        // Filter by user ID
        if (userId.HasValue)
        {
            posts = posts.Where(post =>
                post.UserId == userId.Value);
        }

        // Filter by username
        if (!string.IsNullOrWhiteSpace(userName))
        {
            List<int> matchingUserIds = userRepository
                .GetMany()
                .Where(user =>
                    user.UserName.Contains(userName))
                .Select(user => user.Id)
                .ToList();

            posts = posts.Where(post =>
                matchingUserIds.Contains(post.UserId));
        }

        return Ok(posts.ToList());
    }

    // UPDATE
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, Post post)
    {
        post.Id = id;

        await postRepository.UpdateAsync(post);

        return NoContent();
    }

    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await postRepository.DeleteAsync(id);

        return NoContent();
    }
}