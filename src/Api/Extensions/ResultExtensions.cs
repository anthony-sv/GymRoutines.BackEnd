using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Converts a FluentResults Result into an IResult for minimal API responses.
    /// </summary>
    public static IResult ToApiResult<T>(this Result<T> result, Func<T, IResult>? onSuccess = null)
    {
        if (result.IsSuccess)
            return onSuccess is not null ? onSuccess(result.Value) : Results.Ok(result.Value);

        return result.ToProblemResult();
    }

    public static IResult ToApiResult(this Result result)
    {
        if (result.IsSuccess) return Results.NoContent();
        return result.ToProblemResult();
    }

    private static IResult ToProblemResult(this ResultBase result)
    {
        var firstError = result.Errors.FirstOrDefault();
        var errorCode = firstError?.Metadata.TryGetValue("ErrorCode", out var code) == true
            ? code?.ToString() : null;

        var statusCode = errorCode switch
        {
            "NOT_FOUND" => StatusCodes.Status404NotFound,
            "UNAUTHORIZED" => StatusCodes.Status401Unauthorized,
            "FORBIDDEN" => StatusCodes.Status403Forbidden,
            "CONFLICT" or "EMAIL_EXISTS" or "EXERCISE_NAME_EXISTS" => StatusCodes.Status409Conflict,
            "VALIDATION" or "WEEK_COUNT_MISMATCH" or "DUPLICATE_DAY" or "INVALID_BLOCK_TYPE"
                => StatusCodes.Status400BadRequest,
            "INVALID_CREDENTIALS" => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status400BadRequest
        };

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = firstError?.Message ?? "An error occurred."
        };

        if (result.Errors.Count > 1)
        {
            problem.Extensions["errors"] = result.Errors
                .Select(e => new { message = e.Message, metadata = e.Metadata })
                .ToArray();
        }

        return Results.Problem(problem);
    }

    private static string GetTitle(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        _ => "Error"
    };
}