namespace Blog.Domain.Entities;

public class User : BaseIntEntity
{
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
