using System.Linq;
using AspNet.Dev.Pkg.Infrastructure.Entity;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IBaseGet
    {
        IQueryable GetExpression(IQueryable query) => query;
    }
    public interface IBaseGet<T> : IBaseGet
    {
        IQueryable<T> GetExpression(IQueryable<T> query) => query;
    }
}
