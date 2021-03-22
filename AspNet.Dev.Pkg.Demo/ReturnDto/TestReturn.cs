using System;
using AspNet.Dev.Pkg.Infrastructure.Dto;

namespace AspNet.Dev.Pkg.Demo
{
    public class TestReturn : BaseReturn
    {
        public string Code { get; set; }
        public int? Times { get; set; }
        public bool? IsNumber { get; set; }
        public DateTime? Future { get; set; }
    }
}
