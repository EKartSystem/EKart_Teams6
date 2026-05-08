using System.Net;
using System.Text.Json;

namespace E_Kart_Application.Exceptions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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

            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context,HttpStatusCode.NotFound,ex.Message);
            }

            catch (BadRequestException ex)
            {
                await HandleExceptionAsync(context,HttpStatusCode.BadRequest,ex.Message);
            }

            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context,HttpStatusCode.Unauthorized,ex.Message);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context,HttpStatusCode.InternalServerError,"Internal Server Error");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context,HttpStatusCode statusCode,string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            };
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}