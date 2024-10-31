using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Trackr.API.Infrastructure.Errors;

public class APIError : ProblemDetails
{
    private string _unhandledErrorCode = "UnhanledErrorCode";
    private HttpContext _ctx;
    private Exception _ex;
    public string Code { get; }

    public string? TraceId
    {
        get
        {
            if (Extensions.TryGetValue("TraceId", out var traceId))
            {
                return (string?)traceId;
            }
            return null;
        }
        set => Extensions["TraceId"] = value;
    }

    public APIError(HttpContext ctx, Exception ex)
    {
        _ctx = ctx;
        _ex = ex;
        Code = _unhandledErrorCode;
        Title = ex.Message;
        Status = (int)HttpStatusCode.InternalServerError;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";

        HandleException((dynamic)ex);
    }

    public void HandleException(Exception ex)
    {

    }
    


}
