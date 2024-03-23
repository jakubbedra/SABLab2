using Blog.DAL.Model;
using TDD.DbTestHelpers.Yaml;

namespace Blog.DAL.Tests.Fixtures;

public class BlogFixturesModel
{
    public FixtureTable<Post> Posts { get; set; }
    public FixtureTable<Comment> Comments { get; set; }
}
