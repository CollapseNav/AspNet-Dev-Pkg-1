using System;
using AspNet.Dev.Pkg.Infrastructure.Entity;

namespace AspNet.Dev.Pkg.Demo
{
    public class DemoAgain : BaseEntity<Guid?>
    {
        public string NameAgain { get; set; }
        public int? AgeAgain { get; set; }
    }
}
