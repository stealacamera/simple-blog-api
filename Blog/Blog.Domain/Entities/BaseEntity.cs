namespace Blog.Domain.Entities;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; }
}

public abstract class BaseSimpleEntity<TKey> : BaseEntity where TKey : struct, IComparable<TKey>
{
    public TKey Id { get; set; }
}

public abstract class BaseIntEntity : BaseSimpleEntity<int> { }