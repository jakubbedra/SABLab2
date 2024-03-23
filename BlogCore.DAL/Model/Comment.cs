using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Model;

public class Comment
{
    [Key]
    public long Id { get; set; }

    public string Content { get; set; }
    
    [ForeignKey("Post")]
    public long PostId { get; set; }
    public Post Post { get; set; }
}