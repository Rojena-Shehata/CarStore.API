using CarStore.Shared.CommonResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarStore.Web.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this Result result)
        {
            if (result.IsSucceeded)
                return new OkResult();

            return MapErrors(result.Errors);
        }

        public static IActionResult ToActionResult<TValue>(this Result<TValue> result)
        {
            if (result.IsSucceeded)
                return new OkObjectResult(result.Value);

            return MapErrors(result.Errors);
        }

        private static IActionResult MapErrors(IReadOnlyList<Error> errors)
        {
            if (errors.Count == 0)
            {
                return new ObjectResult(new ProblemDetails
                {
                         Title = "Unexpected Error Occurred",
                         Status = StatusCodes.Status500InternalServerError
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            if (errors.All(e => e.Type == ErrorType.Validation))
            {
                return MapValidationErrors(errors);
            }

            return MapSingleError(errors[0]);
        }

        private static IActionResult MapValidationErrors(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }

            return new BadRequestObjectResult(modelState);
        }

        private static IActionResult MapSingleError(Error error)
        {
            var statusCode = GetStatusCodeByErrorType(error.Type);

            var problemDetails = new ProblemDetails
            {
                Title = error.Code,
                Detail = error.Description,
                Status = statusCode,
                Type = error.Type.ToString()
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
        }

        private static int GetStatusCodeByErrorType(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}
