using System;
using AspNet.Dev.Pkg.Infrastructure.Entity;

namespace AspNet.Dev.Pkg.Demo
{
    public class Test : BaseEntity<Guid?>
    {
        public string Code { get; set; }
        public int? Times { get; set; }
        public bool? IsNumber { get; set; }
        public DateTime? Future { get; set; }
    }
}
