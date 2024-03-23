using Blog.DAL.Infrastructure;
using Blog.DAL.Model;

namespace Blog.DAL.Repository;

public class BlogRepository
{
    private readonly BlogContext _context;

    public BlogRepository(string connectionString)
    {
        _context = new BlogContext(connectionString);
    }

    public void AddPost(Post post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();
    }

    public IEnumerable<Post> GetAllPosts() => _context.Posts;

    public void AddComment(Comment comment)
    {
        _context.Comments.Add(comment);
        _context.SaveChanges();
    }

    public IEnumerable<Comment> GetAllComments() => _context.Comments;

    public IEnumerable<Comment> GetAllCommentsByPost(long postId) =>
        _context.Comments.Where(c => c.PostId == postId);
}
