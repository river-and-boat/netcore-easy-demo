using System;

namespace UserService.Exception
{
    public class GlobalException : ApplicationException
    {
        public override string Message { get; }
        public int Code { get; }
        public int StatusCode { get; }

        public GlobalException(string message, int code, int statusCode)
        {
            Message = message;
            Code = code;
            StatusCode = statusCode;
        }
    }
}
