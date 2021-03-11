using System.Linq;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Util;

namespace AspNet.Dev.Pkg.Demo
{
    public class DemoAgainGet : BaseGet<DemoAgain>
    {
        public string NameAgain { get; set; }
        public int? AgeAgain { get; set; }
        public override IQueryable<DemoAgain> GetExpression(IQueryable<DemoAgain> query)
        {
            return base.GetExpression(query)
            .WhereIf(NameAgain, item => item.NameAgain.Contains(NameAgain))
            .WhereIf(AgeAgain.HasValue, item => item.AgeAgain == AgeAgain)
            ;
        }
    }
}
