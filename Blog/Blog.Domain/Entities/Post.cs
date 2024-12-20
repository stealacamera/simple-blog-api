namespace Blog.Domain.Entities;

public class Post : BaseIntEntity
{
    public int OwnerId { get; set; }

    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public sbyte PostStatusId { get; set; }

    public DateTime? PublishedAt { get; set; }
}
