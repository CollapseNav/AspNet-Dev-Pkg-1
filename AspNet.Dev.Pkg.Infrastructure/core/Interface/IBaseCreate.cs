using System.Linq;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IBaseCreate
    {
        bool IsExist();
    }
    public interface IBaseCreate<T> : IBaseCreate
    {
        bool IsExist(IQueryable<T> rep);
    }
}
