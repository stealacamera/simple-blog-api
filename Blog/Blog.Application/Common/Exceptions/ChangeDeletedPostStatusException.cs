namespace Blog.Application.Common.Exceptions;

public sealed class ChangeDeletedPostStatusException : BaseException
{
    public ChangeDeletedPostStatusException() : base("Cannot change status of a deleted post") { }
}
