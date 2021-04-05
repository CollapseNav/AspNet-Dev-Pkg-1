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
            var config = ExcelCellOption<TestDto>
            .GenExcelOption("姓名", item => item.Name)
            .Add("年龄", item => item.Age, item => int.Parse(item))
            .Add("性别", item => item.Gender, item => item == "男")
            .Add("身高", item => item.Height, item => double.Parse(item));

            return ExcelOperation.ExcelToEntity(file.OpenReadStream(), config);
        }
    }
}
