using System.Reflection;
using Ardalis.SmartEnum;

namespace Blog.Domain.Common.Enums;

public sealed class PostStatuses: SmartEnum<PostStatuses, sbyte>
{
    public static readonly PostStatuses Deleted = new PostStatuses(1, "Deleted", -1),
                                        Public = new PostStatuses(2, "Public", 1),
                                        Draft = new PostStatuses(3, "Draft", 0);

    public sbyte Id { get; }
    private static readonly Dictionary<sbyte, PostStatuses> _fromId = GetAll().ToDictionary(e => e.Id);

    private PostStatuses(sbyte id, string name, sbyte value) : base(name, value)
        => Id = id;

    public static PostStatuses FromId(sbyte id)
        => _fromId.ContainsKey(id) ? _fromId[id] : throw new ArgumentException("Invalid enum id");

    private static IEnumerable<PostStatuses> GetAll() =>
        typeof(PostStatuses).GetFields(BindingFlags.Public |
                                       BindingFlags.Static |
                                       BindingFlags.DeclaredOnly)
                            .Select(f => f.GetValue(null))
                            .Cast<PostStatuses>();
}