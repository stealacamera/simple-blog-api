namespace Blog.Domain.Entities;

public class PostCategory : BaseEntity
{
    public int PostId { get; set; }
    public int CategoryId { get; set; }
}
