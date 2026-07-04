using GestionMicrofinance.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GestionMicrofinance.Api.Middleware;

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
        catch (Exception exception)
        {
            var (statusCode, title) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Ressource non trouvée"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflit"),
                ValidationAppException => (StatusCodes.Status400BadRequest, "Requête invalide"),
                UnauthorizedAppException => (StatusCodes.Status401Unauthorized, "Non autorisé"),
                _ => (StatusCodes.Status500InternalServerError, "Erreur interne")
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "Erreur non gérée");
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
