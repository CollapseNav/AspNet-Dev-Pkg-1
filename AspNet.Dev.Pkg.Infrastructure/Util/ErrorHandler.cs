using System;
using System.Text.Json;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace AspNet.Dev.Pkg.Infrastructure.Util
{
    public class ErrorHandler
    {
        public static async Task ExceptionHandler(HttpContext httpContext, Func<Task> next)
        {
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;
            if (ex != null)
            {
                httpContext.Response.ContentType = "application/problem+json";
                var result = new BaseResultModel
                {
                    Code = 500,
                    Msg = ex.Message
                };
                var stream = httpContext.Response.Body;
                await JsonSerializer.SerializeAsync(stream, result);
            }
        }
    }
}
