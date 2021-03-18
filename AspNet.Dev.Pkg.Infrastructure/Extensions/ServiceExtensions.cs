using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Application;
using AspNet.Dev.Pkg.Infrastructure.Repository;
using System;

namespace AspNet.Dev.Pkg.Infrastructure.Ext
{
    public static class ServiceExtensions
    {
        public static void ConfigureCoreRedis(this IServiceCollection services, string connStr)
        {
            if (!string.IsNullOrEmpty(connStr))
            {
                var conn = ConnectionMultiplexer.Connect(connStr);
                services.AddSingleton(conn);
            }
        }

        public static void ConfigureCoreHttpContext(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void ConfigureCoreScope(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseApplication<,>), typeof(BaseApplication<,>));
            services.AddScoped(typeof(ICrudApplication<,,>), typeof(CrudApplication<,,>));
            services.AddScoped(typeof(IQCrudApplication<,,>), typeof(QCrudApplication<,,>));
        }

        public static void ConfigureCoreRepository<T>(this IServiceCollection services)
        {
            services.ConfigureCoreRepository(typeof(T));
        }

        public static void ConfigureCoreRepository(this IServiceCollection services, Type repo)
        {
            // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepository<>), repo);
        }
    }
}
