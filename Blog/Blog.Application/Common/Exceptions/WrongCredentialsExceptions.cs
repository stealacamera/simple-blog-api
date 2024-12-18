namespace Blog.Application.Common.Exceptions;

public sealed class WrongCredentialsExceptions : BaseException
{
    public WrongCredentialsExceptions() : base("Incorrect email and/or password") { }
}
