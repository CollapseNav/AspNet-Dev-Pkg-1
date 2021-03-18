using AspNet.Dev.Pkg.Infrastructure.Application;
using AspNet.Dev.Pkg.Infrastructure.Controller;
using AspNet.Dev.Pkg.Infrastructure.Interceptor;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Repository;
using AspNet.Dev.Pkg.Infrastructure.UnitOfWork;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Dev.Pkg.Infrastructure.Util
{
    public class BaseAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Application"));
            builder.RegisterGeneric(typeof(Repository<>))
            .As(typeof(IRepository<>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(BaseApplication<,>))
            .As(typeof(IBaseApplication<,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(CrudApplication<,,>))
            .As(typeof(ICrudApplication<,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(QCrudApplication<,,>))
            .As(typeof(IQCrudApplication<,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(BaseController<,>))
            .As(typeof(IBaseController<,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(CrudController<,,>))
            .As(typeof(ICrudController<,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(QCrudController<,,>))
            .As(typeof(IQCrudController<,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
        }
    }
}
