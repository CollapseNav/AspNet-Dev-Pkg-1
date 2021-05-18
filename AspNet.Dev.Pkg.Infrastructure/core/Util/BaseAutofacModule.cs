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
            builder.RegisterGeneric(typeof(ReadonlyRepository<>))
            .As(typeof(IReadonlyRepository<>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(ModifyRepository<>))
            .As(typeof(IModifyRepository<>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();


            builder.RegisterGeneric(typeof(ReadonlyApplication<,>))
            .As(typeof(IReadonlyApplication<,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            builder.RegisterGeneric(typeof(ModifyApplication<,>))
            .As(typeof(IModifyApplication<,>)).InstancePerLifetimeScope().EnableInterfaceInterceptors();
        }
    }
}
