namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly CreatePostView createPostView;
    private readonly ListPostsView listPostsView;
    private readonly SinglePostView singlePostView;

    public ManagePostsView(
        CreatePostView createPostView,
        ListPostsView listPostsView,
        SinglePostView singlePostView)
    {
        this.createPostView = createPostView;
        this.listPostsView = listPostsView;
        this.singlePostView = singlePostView;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Manage Posts ===");
            Console.WriteLine("1. Create post");
            Console.WriteLine("2. List posts");
            Console.WriteLine("3. View specific post");
            Console.WriteLine("0. Back");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await createPostView.ShowAsync();
                    break;

                case "2":
                    listPostsView.Show();
                    break;

                case "3":
                    await singlePostView.ShowAsync();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}