using AspNet.Dev.Pkg.Infrastructure.Dto;

namespace AspNet.Dev.Pkg.Demo
{
    public class DemoCreate : BaseCreate
    {
        public string Name { get; set; }
        public int? Age { get; set; }
    }
}
