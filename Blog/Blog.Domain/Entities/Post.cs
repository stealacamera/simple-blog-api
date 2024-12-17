﻿namespace Blog.Domain.Entities;

public class Post : BaseIntEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public int PostStatusId { get; set; }

    public DateTime PublishedAt { get; set; }
}
