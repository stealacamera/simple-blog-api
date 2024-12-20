namespace Blog.Application.Common.Exceptions;

public sealed class CategoryHasPostsException : BaseException
{
    public CategoryHasPostsException() : base("Action cannot be performed because posts are linked to the specified category") { }
}
