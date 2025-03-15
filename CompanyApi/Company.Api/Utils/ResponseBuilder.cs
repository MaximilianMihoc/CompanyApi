using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Company.Api.Utils
{
    public class ResponseBuilder<T>
    {
        private bool success;
        private T? data;
        private string? message;
        private HttpStatusCode statusCode;

        public T? Data => data;
        public bool IsSuccess => success;
        public string? Message => message;

        public ResponseBuilder<T> WithSuccess(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            success = true;
            this.data = data;
            this.statusCode = statusCode;
            return this;
        }

        public ResponseBuilder<T> WithError(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound)
        {
            success = false;
            this.message = message;
            this.statusCode = statusCode;
            return this;
        }

        public IActionResult Build(ControllerBase controller, string actionName, ILogger logger)
        {
            if (success)
            {
                logger.LogInformation("Action {Action} returned success with status {StatusCode}", actionName, statusCode);
                return controller.StatusCode((int)statusCode, data);
            }
            else
            {
                var errorResponse = new ErrorResponse(message ?? "An error occurred", (int)statusCode);

                logger.LogWarning("Action {Action} failed with status {StatusCode}: {Message}", actionName, statusCode, message);
                return controller.StatusCode((int)statusCode, errorResponse);
            }
        }
    }

    public record ErrorResponse(string Message, int StatusCode);
}
