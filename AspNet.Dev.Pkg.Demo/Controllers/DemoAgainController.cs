using AspNet.Dev.Pkg.Infrastructure.Controller;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNet.Dev.Pkg.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoAgainController : QCrudController<DemoAgain, DemoAgainGet, DemoAgainCreate>
    {
        public DemoAgainController(ILogger<DemoAgainController> logger, IQCrudApplication<DemoAgain, DemoAgainGet, DemoAgainCreate> app) : base(logger, app)
        {
        }
    }
}
