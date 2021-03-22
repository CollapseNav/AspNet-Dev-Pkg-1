using System;
using System.Linq;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Util;

namespace AspNet.Dev.Pkg.Demo
{
    public class TestGet : IBaseGet<Test>
    {
        public string Code { get; set; }
        public int? Times { get; set; }
        public bool? IsNumber { get; set; }
        public DateTime? Future { get; set; }

        public virtual IQueryable<Test> GetExpression(IQueryable<Test> query)
        {
            return query
            .WhereIf(Code, item => item.Code.Contains(Code))
            .WhereIf(Times.HasValue, item => item.Times == Times)
            .WhereIf(IsNumber.HasValue, item => item.IsNumber == IsNumber)
            .WhereIf(Future.HasValue, item => (item.Future ?? DateTime.Now) == Future.Value.Date)
            ;
        }
    }
}
