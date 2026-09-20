using CLI.UI;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using FileRepositories;
using RepositoryContracts;

IUserRepository userRepository = new UserFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();
IPostRepository postRepository = new PostFileRepository();

CreatePostView createPostView =
    new CreatePostView(
        userRepository,
        postRepository);

ListPostsView listPostsView =
    new ListPostsView(postRepository);

SinglePostView singlePostView =
    new SinglePostView(
        postRepository,
        userRepository,
        commentRepository);

ManagePostsView managePostsView =
    new ManagePostsView(
        createPostView,
        listPostsView,
        singlePostView);

CreateUserView createUserView =
    new CreateUserView(userRepository);

ListUsersView listUsersView =
    new ListUsersView(userRepository);

ManageUsersView manageUsersView =
    new ManageUsersView(
        createUserView,
        listUsersView);

CliApp cliApp =
    new CliApp(
        managePostsView,
        manageUsersView);

await cliApp.StartAsync();