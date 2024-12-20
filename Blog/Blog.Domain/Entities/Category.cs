namespace Blog.Domain.Entities;

public class Category : BaseIntEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
