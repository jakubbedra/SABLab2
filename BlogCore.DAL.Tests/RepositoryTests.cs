using Blog.DAL.Model;
using Blog.DAL.Repository;
using Blog.DAL.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using TDD.DbTestHelpers.Core;

namespace Blog.DAL.Tests;

[TestFixture]
public class RepositoryTests : DbBaseTest<BlogFixtures>
{
    [Test, Order(1)]
    public void GetAllPost_TwoPostInDb_ReturnTwoPost()
    {
        // arrange
        // String connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("BloggingDatabase");

        String connectionString = "Server=localhost;Database=master;Trusted_Connection=True;encrypt=false;";
        // //String connectionString = GetConnectionString("BloggingDatabase");
        // var context = new BlogContext(connectionString);
        // context.Database.EnsureCreated();

        var repository = new BlogRepository(connectionString);

        // act
        var result = repository.GetAllPosts();
        // assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }
    
    [Test, Order(2)]
    public void AddPost_AuthorAndContentMissing_ExceptionThrown()
    {
        // arrange
        String connectionString = "Server=localhost;Database=master;Trusted_Connection=True;encrypt=false;";
        var repository = new BlogRepository(connectionString);
        Post post = new Post();
        
        // act
        Action action = () => repository.AddPost(post);
        
        // assert
        Assert.Throws<DbUpdateException>(action.Invoke);
    }
    
    [Test, Order(3)]
    public void AddPost_AuthorMissing_ExceptionThrown()
    {
        // arrange
        String connectionString = "Server=localhost;Database=master;Trusted_Connection=True;encrypt=false;";
        var repository = new BlogRepository(connectionString);
        Post post = new Post() { Content = "sample text" };
        
        // act
        Action action = () => repository.AddPost(post);
        
        // assert
        Assert.Throws<DbUpdateException>(action.Invoke);
    }
    
    [Test, Order(4)]
    public void AddPost_ContentMissing_ExceptionThrown()
    {
        // arrange
        String connectionString = "Server=localhost;Database=master;Trusted_Connection=True;encrypt=false;";
        var repository = new BlogRepository(connectionString);
        Post post = new Post() { Author = "Tomasz G." };
        
        // act
        Action action = () => repository.AddPost(post);
        
        // assert
        Assert.Throws<DbUpdateException>(action.Invoke);
    }
    
    [Test, Order(5)]
    public void AddPost_ValidPost_NoException()
    {
        // arrange
        String connectionString = "Server=localhost;Database=master;Trusted_Connection=True;encrypt=false;";
        var repository = new BlogRepository(connectionString);
        Post post = new Post() { Author = "Tomasz G.", Content = "sample text"};
        
        // act
        Action action = () => repository.AddPost(post);
        
        // assert
        Assert.DoesNotThrow(action.Invoke);
    }
    
    [Test, Order(6)]
    public void GetAllPost_ThreePostInDb_ReturnThreePost()
    {
        // arrange
        String connectionString = "Server=localhost;Database=master;Trusted_Connection=True;encrypt=false;";
        var repository = new BlogRepository(connectionString);

        // act
        var result = repository.GetAllPosts();
        
        // assert
        Assert.That(result.Count(), Is.EqualTo(3));
    }
    
    [Test, Order(7)]
    public void GetAllComments_ThreeCommentsInDb_ReturnThreeComments()
    {
        // arrange
        String connectionString = "Server=localhost;Database=master;Trusted_Connection=True;encrypt=false;";
        var repository = new BlogRepository(connectionString);

        // act
        var result = repository.GetAllComments();
        
        // assert
        Assert.That(result.Count(), Is.EqualTo(3));
    }
    
    [Test, Order(8)]
    public void GetAllCommentsByPost_ThreeCommentsInDb_ReturnTwoComments()
    {
        // arrange
        String connectionString = "Server=localhost;Database=master;Trusted_Connection=True;encrypt=false;";
        var repository = new BlogRepository(connectionString);
        
        // act
        long firstPostId = repository.GetAllPosts().First().Id;
        var result = repository.GetAllCommentsByPost(firstPostId);
        
        // assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }
    
    [Test, Order(9)]
    public void AddComment_SampleComment()
    {
        // arrange
        String connectionString = "Server=localhost;Database=master;Trusted_Connection=True;encrypt=false;";
        var repository = new BlogRepository(connectionString);
        Comment comment = new Comment()
        {
            Content = "hihi"
        };

        // act
        long firstPostId = repository.GetAllPosts().First().Id;
        comment.PostId = firstPostId;
        Action action = () => repository.AddComment(comment);

        // assert
        Assert.DoesNotThrow(action.Invoke);
    }
    
    [Test, Order(10)]
    public void GetAllCommentsByPost_FourCommentsInDb_ReturnThreeComments()
    {
        // arrange
        String connectionString = "Server=localhost;Database=master;Trusted_Connection=True;encrypt=false;";
        var repository = new BlogRepository(connectionString);
        
        // act
        long firstPostId = repository.GetAllPosts().First().Id;
        var result = repository.GetAllCommentsByPost(firstPostId);
        
        // assert
        Assert.That(result.Count(), Is.EqualTo(3));
    }

}
