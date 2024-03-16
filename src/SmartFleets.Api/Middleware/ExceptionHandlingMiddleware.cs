using SmartFleets.Application.Exceptions;
using System.Text.Json;

namespace Web.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions and providing a consistent error response format.
    /// </summary>
    internal sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="logger">The logger for logging error messages.</param>
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

        /// <summary>
        /// Invokes the middleware to handle exceptions.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception occurred.");
                await HandleExceptionAsync(context, e);
            }
        }

        /// <summary>
        /// Handles the exception and writes the error response to the HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var response = new
            {
                source = GetSource(exception),
                status = statusCode,
                detail = exception.Message,
                errors = GetErrors(exception)
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        /// <summary>
        /// Gets the HTTP status code corresponding to the exception type.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>The HTTP status code.</returns>
        private static int GetStatusCode(Exception exception) => exception switch
        {
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

        /// <summary>
        /// Gets the source of the exception.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>The source of the exception.</returns>
        private static string? GetSource(Exception exception) => exception switch
        {
            ApplicationException applicationException => applicationException.Source,
            _ => "Server Error"
        };

        /// <summary>
        /// Gets the validation errors from the exception.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A dictionary of validation errors.</returns>
        private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
        {
            return ((ValidationException)exception).Errors;
        }
    }
}
