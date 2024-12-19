using System.Security.Claims;
using Blog.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected readonly IServicesManager _servicesManager;

    protected BaseController(IServicesManager servicesManager)
        => _servicesManager = servicesManager;

    protected int GetRequesterId()
    {
        var requesterId = User.FindFirst(ClaimTypes.NameIdentifier);

        return requesterId != null ? int.Parse(requesterId.Value) : 0;
    }
}
