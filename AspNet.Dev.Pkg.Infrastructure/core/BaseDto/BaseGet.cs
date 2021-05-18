using System;
using System.Linq;
using AspNet.Dev.Pkg.Infrastructure.Interface;

namespace AspNet.Dev.Pkg.Infrastructure.Dto
{
    public class BaseGet : IBaseGet
    {
        public Guid? Id { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
    public class BaseGet<T> : BaseGet, IBaseGet<T> where T : IBaseEntity
    {
        public virtual IQueryable<T> GetExpression(IQueryable<T> query)
        {
            return query.Where(item => item.IsDeleted == IsDeleted)
            ;
        }
    }
}
