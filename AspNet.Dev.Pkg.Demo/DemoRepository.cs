using AspNet.Dev.Pkg.Demo;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Repository;

namespace Elong.BaseCore.Api
{
    public class DemoRepository<T> : Repository<T> where T : class, IBaseEntity, new()
    {
        public DemoRepository(DemoDbContext db) : base(db)
        {
        }
    }
}
