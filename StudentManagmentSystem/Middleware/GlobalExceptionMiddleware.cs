using System.Net;

namespace StudentManagmentSystem.Middleware
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionMiddleware> logger;

        public GlobalExceptionMiddleware(
            RequestDelegate _next,
            ILogger<GlobalExceptionMiddleware> _logger)
        {
            next = _next;
            logger = _logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Unhandled exception while processing {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await WriteErrorResponseAsync(context);
            }
        }

        private static async Task WriteErrorResponseAsync(HttpContext context)
        {
            if (context.Response.HasStarted)
            {
                throw new InvalidOperationException(
                    "The response has already started, so the exception response cannot be written.");
            }

            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new CommonApiResponse<object>
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An unexpected error occurred.",
                Errors = new List<string> { "Please try again later." }
            });
        }
    }
}