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
            .Require("姓名", item => item.Name)
            .Add("年龄", item => item.Age, ExcelToEntityType.Int32)
            .Default(item => item.Height, 233)
            .AddInit(item =>
            {
                item.Name += "hhhhhhhh";
                item.Gender = false;
                return item;
            })
            ;
            var header = ExcelOperation.GenExcelHeaderByOptions(file.OpenReadStream(), config);
            return ExcelOperation.ExcelToEntity(file.OpenReadStream(), config);
        }
    }
}
