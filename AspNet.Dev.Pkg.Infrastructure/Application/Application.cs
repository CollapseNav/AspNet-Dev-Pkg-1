using AutoMapper;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AspNet.Dev.Pkg.Infrastructure.Application
{
    public class Application : IApplication
    {
        protected readonly ConnectionMultiplexer _conn;
        protected readonly IDatabase _cache;
        protected readonly IMapper _mapper;
        public Application()
        {
            var provider = ServiceGet.GetProvider();
            _mapper = provider?.GetService<IMapper>();
            _conn = provider?.GetService<ConnectionMultiplexer>();
            _cache = _conn?.GetDatabase();
        }
    }
}
