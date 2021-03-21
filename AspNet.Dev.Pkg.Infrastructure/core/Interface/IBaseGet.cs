using System.Linq;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IBaseGet { }
    public interface IBaseGet<T> : IBaseGet
    {
        IQueryable<T> GetExpression(IQueryable<T> query);
    }
}
