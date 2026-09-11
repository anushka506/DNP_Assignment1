using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public void Show()
    {
        IQueryable<User> users = userRepository.GetMany();

        if (!users.Any())
        {
            Console.WriteLine("No users found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("=== Users ===");

        foreach (User user in users)
        {
            Console.WriteLine($"ID: {user.Id}");
            Console.WriteLine($"Username: {user.UserName}");
            Console.WriteLine("--------------------");
        }
    }
}