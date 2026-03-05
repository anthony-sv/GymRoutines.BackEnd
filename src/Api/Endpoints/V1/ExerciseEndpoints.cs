using Application.Features.Exercises.DTOs;
using Asp.Versioning;
using Carter;
using MediatR;
using Api.Extensions;

namespace Api.Endpoints.V1;

public sealed class ExerciseEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        // ─── Exercises ───────────────────────────────────────────────────────
        var exercises = app.MapGroup("/api/v{version:apiVersion}/exercises")
            .WithApiVersionSet(versionSet)
            .WithTags("Exercises")
            .WithOpenApi();

        exercises.MapGet("/", async (
            ISender sender,
            CancellationToken ct,
            string? search = null,
            int page = 1,
            int pageSize = 50) =>
        {
            var result = await sender.Send(new GetExercisesQuery(search, page, pageSize), ct);
            return result.ToApiResult();
        })
        .WithSummary("Get all exercises (paged, searchable, cached)");

        exercises.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetExerciseByIdQuery(id), ct);
            return result.ToApiResult();
        })
        .Produces<ExerciseDto>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get exercise by ID");

        exercises.MapPost("/", async (CreateExerciseCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToApiResult(dto => Results.Created($"/api/v1/exercises/{dto.Id}", dto));
        })
        .RequireAuthorization()
        .Produces<ExerciseDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .WithSummary("Create a custom exercise");

        exercises.MapPut("/{id:guid}", async (
            Guid id,
            UpdateExerciseCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command with { Id = id }, ct);
            return result.ToApiResult();
        })
        .RequireAuthorization()
        .Produces<ExerciseDto>()
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update a custom exercise (cannot modify seeded)");

        exercises.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DeleteExerciseCommand(id), ct);
            return result.ToApiResult();
        })
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete a custom exercise (cannot delete seeded)");

        // ─── Equipment ────────────────────────────────────────────────────────
        var equipment = app.MapGroup("/api/v{version:apiVersion}/equipment")
            .WithApiVersionSet(versionSet)
            .WithTags("Equipment")
            .WithOpenApi();

        equipment.MapGet("/", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetEquipmentQuery(), ct);
            return result.ToApiResult();
        })
        .WithSummary("Get all equipment with variants (cached 1h)");
    }
}