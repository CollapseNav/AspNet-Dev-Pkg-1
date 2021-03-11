using System;
namespace AspNet.Dev.Pkg.Infrastructure.Util
{
    public class ServiceGet
    {
        private static IServiceProvider Provider;
        private ServiceGet() { }
        public static void SetProvider(IServiceProvider provider)
        {
            Provider = provider;
        }

        public static IServiceProvider GetProvider()
        {
            return Provider;
        }

    }
}
