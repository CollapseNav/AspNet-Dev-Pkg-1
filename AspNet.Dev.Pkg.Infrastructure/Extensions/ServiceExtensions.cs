using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Application;
using System;
using System.Collections.Generic;
using AspNet.Dev.Pkg.Infrastructure.Repository;
using AspNet.Dev.Pkg.Infrastructure.Controller;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Dev.Pkg.Infrastructure.Ext
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 注册 Redis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connStr"></param>
        public static void ConfigureCoreRedis(this IServiceCollection services, string connStr)
        {
            if (!string.IsNullOrEmpty(connStr))
            {
                var conn = ConnectionMultiplexer.Connect(connStr);
                services.AddSingleton(conn);
            }
        }

        /// <summary>
        /// 注册 HttpContext
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCoreHttpContext(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void AddDefaultDbContext(this IServiceCollection services, Type context)
        {
            services.AddTransient(typeof(DbContext), context);
        }

        /// <summary>
        /// 默认注册
        /// </summary>
        public static void AddDefaultScope(this IServiceCollection services, Type context = null)
        {
            if (context != null)
                services.AddTransient(typeof(DbContext), context);
            // 注册 仓储 Repository
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IReadonlyRepository<>), typeof(ReadonlyRepository<>));
            services.AddTransient(typeof(IModifyRepository<>), typeof(ModifyRepository<>));

            // 注册 Application
            services.AddTransient(typeof(IApplication), typeof(Application.Application));
            services.AddTransient(typeof(IModifyApplication<,>), typeof(ModifyApplication<,>));
            services.AddTransient(typeof(IReadonlyApplication<,>), typeof(ReadonlyApplication<,>));
            services.AddTransient(typeof(ICrudApplication<,,>), typeof(CrudApplication<,,>));

            // 注册 Controller
            services.AddTransient(typeof(IReadOnlyController<,>), typeof(ReadOnlyController<,>));
            services.AddTransient(typeof(IModifyController<,>), typeof(ModifyController<,>));
            services.AddTransient(typeof(ICrudController<,,>), typeof(CrudController<,,>));
        }
    }
}
