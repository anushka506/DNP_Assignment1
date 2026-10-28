using ApiContracts;

namespace BlazorApp.HttpServices;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId);
}