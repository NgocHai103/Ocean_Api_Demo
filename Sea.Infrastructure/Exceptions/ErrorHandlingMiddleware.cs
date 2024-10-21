using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Sea.Infrastructure.Exceptions;
public class ErrorHandlingMiddleware : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case BusinessException business:
                HandleException(context, business.Code, business.Message, business.Details, business.Data, HttpStatusCode.BadRequest);
                break;
            default:
                HandleException(context, "500", context.Exception.Message, context.Exception.InnerException?.Message, context.Exception.Data);
                break;
        }
    }

    private void HandleException(ExceptionContext context, string code, string message, string detail, object? data = null, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
    {
        var errorResponse = new
        {
            Error = new
            {
                Code = code,
                Message = message,
                Detail = detail,
                Data = data ?? new Dictionary<string, object>()
            }
        };
        context.Result = new ObjectResult(errorResponse)
        {
            StatusCode = (int)httpStatusCode
        };
    }
}