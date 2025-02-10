using System.Net;
using Microsoft.AspNetCore.Mvc;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Result;

namespace WebMarket.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext htppContext)
    {
        try
        {
            await _next(htppContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(htppContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext htppContext, Exception exception)
    {
        _logger.LogError(exception, exception.Message);

        var errorMessage = exception.Message;
        var response = exception switch
        {
            UnauthorizedAccessException _ => new BaseResult() { ErrorMessage = exception.Message, ErrorCode = (int)HttpStatusCode.Unauthorized },
            _ => new BaseResult() { ErrorMessage = "Internal server error", ErrorCode = (int)ErrorCodes.InternalServerError},
        };

        htppContext.Response.ContentType = "application/json";
        htppContext.Response.StatusCode = (int)response.ErrorCode;
        await htppContext.Response.WriteAsJsonAsync(errorMessage);
    }


}