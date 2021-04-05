using AspNet.Dev.Pkg.Infrastructure.Controller;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Dev.Pkg.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : QueryController<Test, TestReturn, TestGet, TestCreate>
    {
        public TestController(IQueryApplication<Test, TestReturn, TestGet, TestCreate> app) : base(app) { }

    }
}
