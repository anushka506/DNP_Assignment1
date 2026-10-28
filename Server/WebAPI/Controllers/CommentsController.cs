using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    // CREATE
    [HttpPost]
    public async Task<ActionResult<Comment>> CreateAsync(Comment comment)
    {
        Comment createdComment =
            await commentRepository.AddAsync(comment);

        return Created(
            $"api/comments/{createdComment.Id}",
            createdComment
        );
    }

    // GET SINGLE
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Comment>> GetSingleAsync(int id)
    {
        Comment comment =
            await commentRepository.GetSingleAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment);
    }

    // GET MANY
    // Examples:
    // GET /api/comments
    // GET /api/comments?userId=1
    // GET /api/comments?postId=2
    // GET /api/comments?userId=1&postId=2
    [HttpGet]
    public ActionResult<IEnumerable<Comment>> GetMany(
        [FromQuery] int? userId,
        [FromQuery] int? postId)
    {
        IQueryable<Comment> comments =
            commentRepository.GetMany();

        if (userId != null)
        {
            comments = comments.Where(comment =>
                comment.UserId == userId);
        }

        if (postId != null)
        {
            comments = comments.Where(comment =>
                comment.PostId == postId);
        }

        return Ok(comments.ToList());
    }

    // UPDATE
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        Comment comment)
    {
        if (id != comment.Id)
        {
            return BadRequest(
                "ID in URL must match comment ID.");
        }

        await commentRepository.UpdateAsync(comment);

        return NoContent();
    }

    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await commentRepository.DeleteAsync(id);

        return NoContent();
    }
}