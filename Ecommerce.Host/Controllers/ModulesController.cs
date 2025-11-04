using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Host.Controllers;
[ApiController]
[Route("api/host/modules")]
public class ModulesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetModules()
    {
        return Ok(HostServicesRegistration.LoadedModules);
    }
}
