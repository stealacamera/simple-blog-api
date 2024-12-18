using Blog.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected readonly IServicesManager _servicesManager;

    protected BaseController(IServicesManager servicesManager)
        => _servicesManager = servicesManager;
}
