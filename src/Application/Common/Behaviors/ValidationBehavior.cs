using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

/// <summary>
/// Runs all FluentValidation validators for a request before the handler.
/// Returns a failed Result if any validation fails.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : ResultBase, new()
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next(cancellationToken);

        logger.LogWarning("Validation failed for {RequestType}: {Errors}",
            typeof(TRequest).Name,
            string.Join("; ", failures.Select(f => f.ErrorMessage)));

        var errors = failures.Select(f =>
            new Error(f.ErrorMessage)
                .WithMetadata("PropertyName", f.PropertyName)
                .WithMetadata("ErrorCode", "VALIDATION"));

        var response = new TResponse();
        response.Reasons.AddRange(errors);
        return response;
    }
}