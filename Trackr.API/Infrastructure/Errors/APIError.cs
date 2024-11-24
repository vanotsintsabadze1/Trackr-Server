using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trackr.Application.Exceptions;

namespace Trackr.API.Infrastructure.Errors;

public class APIError : ProblemDetails
{
    private const string _unhandledErrorCode = "UnhandledErrorCode";
    
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
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
        Code = _unhandledErrorCode;
        Title = ex.Message;
        TraceId = ctx.TraceIdentifier;
        Status = (int)HttpStatusCode.InternalServerError;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";

        HandleException((dynamic)ex);
    }

    private void HandleException(Exception ex)
    {
        
    }

    private void HandleException(UserAlreadyExistsException ex)
    {
        Code = ex.Code;
        Title = ex.Message;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
        Status = (int)HttpStatusCode.Conflict;
    }

    private void HandleException(UserInvalidCredentialsException ex)
    {
        Code = ex.Code;
        Title = ex.Message;
        Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
        Status = (int)HttpStatusCode.Unauthorized;
    }

    private void HandleException(InvalidTransactionException ex)
    {
        Code = ex.Code;
        Title = ex.Message;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
        Status = (int)HttpStatusCode.BadRequest;
    }

    private void HandleException(UserUnauthorizedException ex)
    {
        Code = ex.Code;
        Title = ex.Message;
        Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
        Status = (int)HttpStatusCode.Unauthorized;
    }
}
