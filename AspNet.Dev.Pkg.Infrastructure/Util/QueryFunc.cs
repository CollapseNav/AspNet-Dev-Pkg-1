using System;
using System.Linq;
using System.Linq.Expressions;
using AspNet.Dev.Pkg.Infrastructure.Interface;

namespace AspNet.Dev.Pkg.Infrastructure.Util
{
    public static class QueryClass
    {
        public static IQueryable<T> WhereIf<T>(this IRepository<T> repository, bool flag, Expression<Func<T, bool>> expression) where T : class, IBaseEntity
        {
            var query = repository.GetContext().Set<T>();
            return flag ? query.Where(expression) : query;
        }
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool flag, Expression<Func<T, bool>> expression)
        {
            return flag ? query.Where(expression) : query;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, string flag, Expression<Func<T, bool>> expression)
        {
            return string.IsNullOrEmpty(flag) ? query : query.Where(expression);
        }
    }

}
