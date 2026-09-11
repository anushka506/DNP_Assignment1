namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly CreateUserView createUserView;
    private readonly ListUsersView listUsersView;

    public ManageUsersView(
        CreateUserView createUserView,
        ListUsersView listUsersView)
    {
        this.createUserView = createUserView;
        this.listUsersView = listUsersView;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Manage Users ===");
            Console.WriteLine("1. Create user");
            Console.WriteLine("2. List users");
            Console.WriteLine("0. Back");

            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await createUserView.ShowAsync();
                    break;

                case "2":
                    listUsersView.Show();
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