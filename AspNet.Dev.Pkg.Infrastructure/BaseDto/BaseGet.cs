using System;
using System.Linq;

namespace AspNet.Dev.Pkg.Infrastructure.Dto
{
    public class BaseGet
    {
        public Guid? Id { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public class BaseGet<T>
    {
        public Guid? Id { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual IQueryable<T> GetExpression(IQueryable<T> query)
        {
            return query;
        }
    }
}
