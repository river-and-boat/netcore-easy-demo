using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UserService.Controller.Response;
using UserService.Exception;

namespace UserService.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (GlobalException ex)
            {
                CommonResponse commonResponse = new CommonResponse()
                {
                    Data = ex.Message,
                    Code = ex.Code
                };
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsJsonAsync(commonResponse);
            }
        }
    }
}
