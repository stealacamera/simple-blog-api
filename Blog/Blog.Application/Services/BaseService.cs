using Blog.Application.Abstractions;

namespace Blog.Application.Services;

internal abstract class BaseService
{
    protected readonly IWorkUnit _workUnit;

    protected BaseService(IWorkUnit workUnit)
        => _workUnit = workUnit;
}
