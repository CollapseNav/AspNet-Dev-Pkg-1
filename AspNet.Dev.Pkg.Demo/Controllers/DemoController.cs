using AspNet.Dev.Pkg.Infrastructure.Controller;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNet.Dev.Pkg.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : CrudController<Demo, BaseGet, DemoCreate>
    {
        public DemoController(ILogger<DemoController> logger, ICrudApplication<Demo, BaseGet, DemoCreate> app) : base(logger, app)
        {
        }
    }
}
