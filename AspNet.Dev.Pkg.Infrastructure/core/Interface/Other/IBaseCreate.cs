using System.Linq;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IBaseCreate
    {
    }
    public interface IBaseCreate<T> : IBaseCreate
    {
        bool IsExist(IQueryable<T> rep);
    }
}
