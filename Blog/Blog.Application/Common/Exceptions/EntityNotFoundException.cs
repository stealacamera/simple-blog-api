namespace Blog.Application.Common.Exceptions;

public sealed class EntityNotFoundException : BaseException
{
    public EntityNotFoundException(string entity) : base($"{entity} could not be found") { }
}
