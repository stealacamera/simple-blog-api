using System.Security.Claims;
using Blog.Application.Abstractions;
using Blog.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected readonly IServicesManager _servicesManager;

    protected BaseController(IServicesManager servicesManager)
        => _servicesManager = servicesManager;

    protected int GetRequesterId()
    {
        var requesterId = GetRequesterIdOrAnon();
        return requesterId ?? throw new UnauthorizedException();
    }

    protected int? GetRequesterIdOrAnon()
    {
        var requesterId = User.FindFirst(ClaimTypes.NameIdentifier);
        return requesterId == null ? null : int.Parse(requesterId.Value);
    }
}
