using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trackr.Application.Exceptions;

namespace Trackr.API.Infrastructure.Errors;

public class APIError : ProblemDetails
{
    private string _unhandledErrorCode = "UnhanledErrorCode";
    private HttpContext _ctx;
    private Exception _ex;
    public string Code { get; set; }

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
        TraceId = ctx.TraceIdentifier;
        Status = (int)HttpStatusCode.InternalServerError;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";

        HandleException((dynamic)ex);
    }

    public void HandleException(Exception ex)
    {

    }

    public void HandleException(UserAlreadyExistsException ex)
    {
        Code = ex.Code;
        Title = ex.Message;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
        Status = (int)HttpStatusCode.Conflict;
    }

    public void HandleException(UserInvalidCredentialsException ex)
    {
        Code = ex.Code;
        Title = ex.Message;
        Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
        Status = (int)HttpStatusCode.Unauthorized;
    }


}
