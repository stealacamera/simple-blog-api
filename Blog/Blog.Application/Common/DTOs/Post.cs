﻿using Blog.Domain.Common.Enums;

namespace Blog.Application.Common.DTOs;

public record PostStatus
{
    public sbyte Value { get; }
    public string Name { get; }

    public PostStatus(sbyte value, string name)
    {
        Value = value;
        Name = name;
    }

    public PostStatus(PostStatuses status)
    {
        Value = status.Value;
        Name = status.Name;
    }
}

    public record Post(
    int Id, 
    string Title, 
    string Content, 
    PostStatus Status,
    DateTime CreatedAt,
    DateTime? PublishedAt,
    IList<Category> Categories);