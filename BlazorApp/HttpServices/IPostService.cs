using ApiContracts;

namespace BlazorApp.HttpServices;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<List<PostDto>> GetPostsAsync();
    Task<PostDto> GetPostAsync(int id);
}