using Blog.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Infrastructure;

public class BlogContext : DbContext
{
    private string _connectionString;
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public BlogContext(string connectionString) : base()
    {
        _connectionString = connectionString;
    }

    public BlogContext() : base()
    {
        _connectionString = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;encrypt=false;";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId);
    }
}