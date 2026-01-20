using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using CarStore.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using static CarStore.Domain.Exceptions.CustomExceptions;

namespace CarStore.Domain.ExceptionHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode status;
            object? errors = null;

            switch (exception)
            {
                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    break;
                case BadRequestException:
                    status = HttpStatusCode.BadRequest;
                    break;
                case ValidationException ve:
                    status = HttpStatusCode.BadRequest;
                    errors = ve.Errors;
                    break;
                case UnauthorizedException:
                    status = HttpStatusCode.Unauthorized;
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.StatusCode = (int)status;
            var response = new ExceptionResponse
            {
                Message = exception.Message,
                Errors = errors
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
