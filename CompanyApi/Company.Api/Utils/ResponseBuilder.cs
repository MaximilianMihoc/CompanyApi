using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Company.Api.Utils
{
    public class ResponseBuilder<T>
    {
        private bool _success;
        private T? _data;
        private string? _message;
        private HttpStatusCode _statusCode;

        public ResponseBuilder<T> WithSuccess(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _success = true;
            _data = data;
            _statusCode = statusCode;
            return this;
        }

        public ResponseBuilder<T> WithError(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound)
        {
            _success = false;
            _message = message;
            _statusCode = statusCode;
            return this;
        }

        public IActionResult Build(ControllerBase controller, string actionName, ILogger logger)
        {
            if (_success)
            {
                logger.LogInformation("Action {Action} returned success with status {StatusCode}", actionName, _statusCode);
                return controller.StatusCode((int)_statusCode, _data);
            }
            else
            {
                var errorResponse = new ErrorResponse(_message ?? "An error occurred", (int)_statusCode);

                logger.LogWarning("Action {Action} failed with status {StatusCode}: {Message}", actionName, _statusCode, _message);
                return controller.StatusCode((int)_statusCode, errorResponse);
            }
        }
    }

    public record ErrorResponse(string Message, int StatusCode);
}
