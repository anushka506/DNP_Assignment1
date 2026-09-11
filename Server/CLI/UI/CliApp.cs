using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public CliApp(
        IUserRepository userRepository,
        ICommentRepository commentRepository,
        IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Forum CLI ===");
            Console.WriteLine("1. Create user");
            Console.WriteLine("2. Create post");
            Console.WriteLine("3. Add comment");
            Console.WriteLine("4. View posts");
            Console.WriteLine("5. View specific post");
            Console.WriteLine("0. Exit");

            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreateUserAsync();
                    break;

                case "2":
                    await CreatePostAsync();
                    break;

                case "3":
                    await CreateCommentAsync();
                    break;

                case "4":
                    ViewPosts();
                    break;

                case "5":
                    await ViewSpecificPostAsync();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private async Task CreateUserAsync()
    {
        Console.Write("Enter username: ");
        string? userName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(userName))
        {
            Console.WriteLine("Username cannot be empty.");
            return;
        }

        bool usernameExists = userRepository
            .GetMany()
            .Any(u => u.UserName.ToLower() == userName.ToLower());

        if (usernameExists)
        {
            Console.WriteLine("Username is already taken.");
            return;
        }

        Console.Write("Enter password: ");
        string? password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Password cannot be empty.");
            return;
        }

        User user = new User
        {
            UserName = userName,
            Password = password
        };

        User created = await userRepository.AddAsync(user);

        Console.WriteLine($"User created with ID: {created.Id}");
    }

    private async Task CreatePostAsync()
    {
        Console.Write("Enter title: ");
        string? title = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title cannot be empty.");
            return;
        }

        Console.Write("Enter body: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Body cannot be empty.");
            return;
        }

        Console.Write("Enter user ID: ");
        string? userIdInput = Console.ReadLine();

        if (!int.TryParse(userIdInput, out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }

        bool userExists = userRepository
            .GetMany()
            .Any(u => u.Id == userId);

        if (!userExists)
        {
            Console.WriteLine($"User with ID {userId} does not exist.");
            return;
        }

        Post post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId
        };

        Post created = await postRepository.AddAsync(post);

        Console.WriteLine($"Post created with ID: {created.Id}");
    }

    private async Task CreateCommentAsync()
    {
        Console.Write("Enter comment: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment cannot be empty.");
            return;
        }

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
            Console.WriteLine($"Post with ID {postId} does not exist.");
            return;
        }

        Console.Write("Enter user ID: ");
        string? userIdInput = Console.ReadLine();

        if (!int.TryParse(userIdInput, out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }

        bool userExists = userRepository
            .GetMany()
            .Any(u => u.Id == userId);

        if (!userExists)
        {
            Console.WriteLine($"User with ID {userId} does not exist.");
            return;
        }

        Comment comment = new Comment
        {
            Body = body,
            PostId = postId,
            UserId = userId
        };

        Comment created = await commentRepository.AddAsync(comment);

        Console.WriteLine($"Comment created with ID: {created.Id}");
    }

    private void ViewPosts()
    {
        IQueryable<Post> posts = postRepository.GetMany();

        if (!posts.Any())
        {
            Console.WriteLine("No posts found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("=== Posts ===");

        foreach (Post post in posts)
        {
            Console.WriteLine($"ID: {post.Id}");
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine("--------------------");
        }
    }

    private async Task ViewSpecificPostAsync()
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
            Console.WriteLine($"Post with ID {postId} does not exist.");
            return;
        }

        Post post = await postRepository.GetSingleAsync(postId);

        User author = await userRepository.GetSingleAsync(post.UserId);

        Console.WriteLine();
        Console.WriteLine("=== Post ===");
        Console.WriteLine($"ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine($"Author: {author.UserName}");

        IQueryable<Comment> comments = commentRepository
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
                await userRepository.GetSingleAsync(comment.UserId);

            Console.WriteLine($"Comment ID: {comment.Id}");
            Console.WriteLine($"Author: {commentAuthor.UserName}");
            Console.WriteLine($"Comment: {comment.Body}");
            Console.WriteLine("--------------------");
        }
    }
}