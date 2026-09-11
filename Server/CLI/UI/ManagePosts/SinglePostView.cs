using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;

    public SinglePostView(
        IPostRepository postRepository,
        IUserRepository userRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter post ID: ");
        string? postIdInput = Console.ReadLine();

        if (!int.TryParse(postIdInput, out int postId))
        {
            Console.WriteLine("Invalid post ID.");
            return;
        }

        bool postExists = postRepository
            .GetMany()
            .Any(p => p.Id == postId);

        if (!postExists)
        {
            Console.WriteLine(
                $"Post with ID {postId} does not exist.");

            return;
        }

        Post post =
            await postRepository.GetSingleAsync(postId);

        User author =
            await userRepository.GetSingleAsync(
                post.UserId);

        Console.WriteLine();
        Console.WriteLine("=== Post ===");
        Console.WriteLine($"ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine(
            $"Author: {author.UserName}");

        IQueryable<Comment> comments =
            commentRepository
                .GetMany()
                .Where(c => c.PostId == postId);

        Console.WriteLine();
        Console.WriteLine("=== Comments ===");

        if (!comments.Any())
        {
            Console.WriteLine("No comments.");
            return;
        }

        foreach (Comment comment in comments)
        {
            User commentAuthor =
                await userRepository
                    .GetSingleAsync(
                        comment.UserId);

            Console.WriteLine(
                $"Comment ID: {comment.Id}");

            Console.WriteLine(
                $"Author: {commentAuthor.UserName}");

            Console.WriteLine(
                $"Comment: {comment.Body}");

            Console.WriteLine("--------------------");
        }
    }
}