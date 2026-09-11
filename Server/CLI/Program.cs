using CLI.UI;
using CLI.UI.ManagePosts;
using InMemoryRepositories;
using RepositoryContracts;

IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();

CreatePostView createPostView =
    new CreatePostView(userRepository, postRepository);

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

CliApp cliApp =
    new CliApp(managePostsView);

await cliApp.StartAsync();