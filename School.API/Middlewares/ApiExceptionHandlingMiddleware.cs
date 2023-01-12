using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace School.API.Middlewares;

public class ApiExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionHandlingMiddleware> _logger;

    public ApiExceptionHandlingMiddleware(RequestDelegate next, ILogger<ApiExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var errorResponse = new ErrorResponse()
        {
            Error = ex.Message,
            Status = (int)HttpStatusCode.InternalServerError,
            Title = ex switch
            {
                BadHttpRequestException => "Bad request",
                _ => "Internal Server Error."
            }
        };

        context.Response.StatusCode = errorResponse.Status;
        var result = JsonSerializer.Serialize(errorResponse);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
}