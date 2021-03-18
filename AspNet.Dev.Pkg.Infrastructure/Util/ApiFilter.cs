using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNet.Dev.Pkg.Infrastructure.Util
{
    public class ApiFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var objResult = context.Result as ObjectResult;
            context.Result = new OkObjectResult(new BaseResultModel
            {
                Code = objResult?.StatusCode ?? 200,
                Result = objResult?.Value,
                Msg = "Success"
            });
        }
    }
}
