using CvForgeAI.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace CvForgeAI.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context,
                ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType =
            "application/json";

        var statusCode =
            HttpStatusCode.InternalServerError;

        var message =
            "Internal server error";

        switch (exception)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "Internal server error.";
                break;
        }

        context.Response.StatusCode =
            (int)statusCode;

        var response = new
        {
            success = false,
            message
        };

        var json = JsonSerializer.Serialize(
            response);

        await context.Response.WriteAsync(json);
    }
}