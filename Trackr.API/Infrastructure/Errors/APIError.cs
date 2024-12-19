using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Serialization;
using Trackr.Application.Exceptions;

namespace Trackr.API.Infrastructure.Errors;

public class APIError : ProblemDetails
{
    private const string _unhandledErrorCode = "UnhandledErrorCode";
    private string? traceId;

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("traceId")]
    public string? TraceId
    {
        get
        {
            if (Extensions.TryGetValue("traceId", out var traceId))
            {
                return (string?)traceId;
            }
            return null;
        }
        set => Extensions["traceId"] = value;
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

    private void HandleException(NotFoundException ex)
    {
        Code = ex.Code;
        Title = ex.Message;
        Status = (int)HttpStatusCode.NotFound;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    }

    private void HandleException(ConflictException ex)
    {
        Code = ex.Code;
        Title = ex.Message;
        Status = (int)HttpStatusCode.Conflict;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    }

    private void HandleException(UserAlreadyExistsException ex)
    {
        Code = ex.Code;
        Title = ex.Message;
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
        Status = (int)HttpStatusCode.Conflict;
    }

    private void HandleException(InvalidUserException ex)
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
