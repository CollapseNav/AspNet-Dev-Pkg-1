using System.Collections.Generic;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Dev.Pkg.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExcelController : ControllerBase
    {
        [HttpPost, Route("ExcelImport")]
        public ICollection<TestDto> ExcelImport(IFormFile file)
        {
            var config = new ExcelImportOption<TestDto>()
            .Add("姓名", item => item.Name)
            .Add("年龄", item => item.Age, item => int.Parse(item))
            .Default(item => item.Height, 233)
            .AddInit(item =>
            {
                item.Name += "hhhhhhhh";
                return item;
            })
            ;
            return ExcelOperation.ExcelToEntity(file.OpenReadStream(), config);
        }
    }
}
