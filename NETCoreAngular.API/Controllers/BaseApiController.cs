
using Microsoft.AspNetCore.Mvc;
using NETCoreAngular.API.Heplers;

namespace NETCoreAngular.API.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    
}