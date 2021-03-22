using AspNet.Dev.Pkg.Infrastructure.Application;
using AspNet.Dev.Pkg.Infrastructure.Controller;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Repository;
using Autofac;
using Autofac.Extras.DynamicProxy;

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
            builder.RegisterGeneric(typeof(QueryApplication<,,>))
            .As(typeof(IQueryApplication<,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(BaseController<,>))
            .As(typeof(IBaseController<,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(CrudController<,,>))
            .As(typeof(ICrudController<,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(QueryController<,,>))
            .As(typeof(IQueryController<,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();


            builder.RegisterGeneric(typeof(CrudApplication<,,,>))
            .As(typeof(ICrudApplication<,,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(QueryApplication<,,,>))
            .As(typeof(IQueryApplication<,,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(CrudController<,,,>))
            .As(typeof(ICrudController<,,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(QueryController<,,,>))
            .As(typeof(IQueryController<,,,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
        }
    }
}
