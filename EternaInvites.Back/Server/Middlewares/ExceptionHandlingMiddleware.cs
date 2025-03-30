using Domain.Exceptions;
using System.Net;

namespace Server.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Get request body if needed (for logging)
        string requestBody = string.Empty;
        if (context.Request.Body.CanSeek)
        {
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            exception.Data.Add("Request", requestBody);
        }

        // Determine the status code
        int statusCode = exception is ExceptionHelper exHelper
            ? (int)exHelper.StatusCode
            : (int)HttpStatusCode.InternalServerError;

        // Log the exception
        _logger.LogError(exception,
            $"{context.Request.Path}({statusCode}): {exception.Message}");

        // Set the response
        context.Response.ContentType = "application/text";
        context.Response.StatusCode = statusCode;



        await context.Response.WriteAsync(exception.Message);
    }
}

// Extension method for easy registration in Program.cs
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}