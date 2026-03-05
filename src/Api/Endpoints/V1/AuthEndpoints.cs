using Application.Features.Users.DTOs;
using Asp.Versioning;
using Carter;
using MediatR;
using Api.Extensions;

namespace Api.Endpoints.V1;

public sealed class AuthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("/api/v{version:apiVersion}/auth")
            .WithApiVersionSet(versionSet)
            .WithTags("Authentication")
            .WithOpenApi(); // Check Scalar OpenAPI Extensions

        group.MapPost("/register", async (RegisterCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToApiResult(auth => Results.Created($"/api/v1/auth/me", auth));
        })
        .Produces<AuthResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .WithSummary("Register a new user");

        group.MapPost("/login", async (LoginCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToApiResult();
        })
        .Produces<AuthResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Login and receive JWT tokens");

        group.MapPost("/refresh", async (RefreshTokenCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToApiResult();
        })
        .Produces<AuthResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Refresh access token");

        group.MapGet("/me", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetCurrentUserQuery(), ct);
            return result.ToApiResult();
        })
        .RequireAuthorization()
        .Produces<UserDto>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Get current authenticated user");
    }
}