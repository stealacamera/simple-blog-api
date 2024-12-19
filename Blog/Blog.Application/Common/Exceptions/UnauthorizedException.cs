namespace Blog.Application.Common.Exceptions;

public sealed class UnauthorizedException : BaseException
{
    public UnauthorizedException() : base("You are unauthorized to perform this action") { }
}
