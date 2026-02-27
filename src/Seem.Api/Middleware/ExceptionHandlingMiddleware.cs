using System.Net;
using System.Text.Json;
using FluentValidation;
using Seem.Domain.Exceptions;

namespace Seem.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

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
        var (statusCode, errors) = exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                validationException.Errors.Select(e => e.ErrorMessage).ToArray()
            ),
            DomainException domainException => (
                HttpStatusCode.UnprocessableEntity,
                new[] { domainException.Message }
            ),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                new[] { "Resource not found." }
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                new[] { "An unexpected error occurred." }
            )
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            errors,
            statusCode = (int)statusCode
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
