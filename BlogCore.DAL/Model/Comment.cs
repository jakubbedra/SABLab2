using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Model;

public class Comment
{
    // auto-generated id
    [Key]
    public long Id { get; set; }

    // content of the comment
    public string Content { get; set; }
    
    // the id of the commented post
    [ForeignKey("Post")]
    public long PostId { get; set; }
    
    // commented post
    public Post Post { get; set; }
}