using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;

namespace CLI.UI;

public class CliApp
{
    private readonly ManagePostsView managePostsView;
    private readonly ManageUsersView manageUsersView;

    public CliApp(
        ManagePostsView managePostsView,
        ManageUsersView manageUsersView)
    {
        this.managePostsView = managePostsView;
        this.manageUsersView = manageUsersView;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Forum CLI ===");
            Console.WriteLine("1. Manage posts");
            Console.WriteLine("2. Manage users");
            Console.WriteLine("0. Exit");

            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await managePostsView.ShowAsync();
                    break;

                case "2":
                    await manageUsersView.ShowAsync();
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