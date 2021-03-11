using System.Linq;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Util;

namespace AspNet.Dev.Pkg.Demo
{
    public class DemoGet : BaseGet<Demo>
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public override IQueryable<Demo> GetExpression(IQueryable<Demo> query)
        {
            return base.GetExpression(query)
            .WhereIf(Name, item => item.Name.Contains(Name))
            .WhereIf(Age.HasValue, item => item.Age == Age)
            ;
        }
    }
}
