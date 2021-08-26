using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserService.Controller.Response;

namespace UserService.Filter
{
    public class ResponseWrapperFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            HandleObjectResult(context);
            HandleJsonResult(context);
        }

        private void HandleObjectResult(ResultExecutingContext context)
        {
            var result = context.Result;
            if (result is not ObjectResult)
            {
                return;
            }
            var objectResult = result as ObjectResult;
            objectResult.Value = new CommonResponse()
            {
                Data = objectResult.Value,
                Code = context.HttpContext.Response.StatusCode
            };
        }

        private void HandleJsonResult(ResultExecutingContext context)
        {
            var result = context.Result;
            if (result is not JsonResult)
            {
                return;
            }
            var jsonResult = result as JsonResult;
            jsonResult.Value = new CommonResponse()
            {
                Data = jsonResult.Value,
                Code = context.HttpContext.Response.StatusCode
            };
        }
    }
}
