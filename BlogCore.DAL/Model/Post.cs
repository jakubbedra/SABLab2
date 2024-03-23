using System.ComponentModel.DataAnnotations;

namespace Blog.DAL.Model
{
    public class Post
    {
        // auto-generated id
        [Key]
        public long Id { get; set; }

        // the content of a post
        [Required]
        public string Content { get; set; }

        // the name of the author
        [Required]
        public string Author { get; set; }
        
        // related comments
        public List<Comment> Comments { get; set; }
    }
}
