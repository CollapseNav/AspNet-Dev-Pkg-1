using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.AspNetCore.Builder;

namespace AspNet.Dev.Pkg.Infrastructure.Ext
{
    public static class MiddlewareExtensions
    {
        public static void UseCoreServiceGet(this IApplicationBuilder app)
        {
            ServiceGet.SetProvider(app.ApplicationServices);
        }

    }
}
