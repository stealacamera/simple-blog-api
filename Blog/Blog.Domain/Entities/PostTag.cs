namespace Blog.Domain.Entities;

public class PostTag : BaseEntity
{
    public int PostId { get; set; }
    public int TagId { get; set; }
}
