using Application.Features.TrainingPlans.DTOs;
using Asp.Versioning;
using Carter;
using MediatR;
using Api.Extensions;

namespace Api.Endpoints.V1;

public sealed class TrainingPlanEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        // ─── Training Plans ───────────────────────────────────────────────────
        var plans = app.MapGroup("/api/v{version:apiVersion}/training-plans")
            .WithApiVersionSet(versionSet)
            .WithTags("Training Plans")
            .RequireAuthorization()
            .WithOpenApi();

        plans.MapGet("/", async (
            ISender sender,
            CancellationToken ct,
            int page = 1,
            int pageSize = 20,
            bool includePublic = false) =>
        {
            var result = await sender.Send(new GetTrainingPlansQuery(page, pageSize, includePublic), ct);
            return result.ToApiResult();
        })
        .WithSummary("Get my training plans (paged)");

        plans.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetTrainingPlanDetailQuery(id), ct);
            return result.ToApiResult();
        })
        .Produces<TrainingPlanDetailDto>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get full training plan details with all weeks and days");

        plans.MapPost("/", async (CreateTrainingPlanCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToApiResult(dto => Results.Created($"/api/v1/training-plans/{dto.Id}", dto));
        })
        .Produces<TrainingPlanSummaryDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new training plan");

        plans.MapPut("/{id:guid}", async (
            Guid id,
            UpdateTrainingPlanCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command with { Id = id }, ct);
            return result.ToApiResult();
        })
        .Produces<TrainingPlanSummaryDto>()
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update training plan metadata");

        plans.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DeleteTrainingPlanCommand(id), ct);
            return result.ToApiResult();
        })
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .WithSummary("Delete a training plan");

        // ─── Week Templates ───────────────────────────────────────────────────
        var weeks = app.MapGroup("/api/v{version:apiVersion}/training-plans/{planId:guid}/weeks")
            .WithApiVersionSet(versionSet)
            .WithTags("Week Templates")
            .RequireAuthorization()
            .WithOpenApi();

        weeks.MapPost("/", async (
            Guid planId,
            AddWeekTemplateRequest request,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(new AddWeekTemplateCommand(planId, request.WeekNumber), ct);
            return result.ToApiResult(dto => Results.Created(
                $"/api/v1/training-plans/{planId}/weeks/{dto.Id}", dto));
        })
        .Produces<WeekTemplateDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add a week template to a training plan");

        // ─── Workout Days ─────────────────────────────────────────────────────
        var days = app.MapGroup("/api/v{version:apiVersion}/week-templates/{weekId:guid}/days")
            .WithApiVersionSet(versionSet)
            .WithTags("Workout Days")
            .RequireAuthorization()
            .WithOpenApi();

        days.MapPost("/", async (
            Guid weekId,
            AddWorkoutDayCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command with { WeekTemplateId = weekId }, ct);
            return result.ToApiResult(dto => Results.Created(
                $"/api/v1/workout-days/{dto.Id}", dto));
        })
        .Produces<WorkoutDaySummaryDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add a day to a week template");

        // ─── Workout Days (by ID) ─────────────────────────────────────────────
        var workoutDays = app.MapGroup("/api/v{version:apiVersion}/workout-days")
            .WithApiVersionSet(versionSet)
            .WithTags("Workout Days")
            .RequireAuthorization()
            .WithOpenApi();

        workoutDays.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetWorkoutDayDetailQuery(id), ct);
            return result.ToApiResult();
        })
        .Produces<WorkoutDayDetailDto>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get full workout day with all blocks");

        // ─── Blocks ───────────────────────────────────────────────────────────
        var blocks = app.MapGroup("/api/v{version:apiVersion}/workout-days/{dayId:guid}/blocks")
            .WithApiVersionSet(versionSet)
            .WithTags("Workout Blocks")
            .RequireAuthorization()
            .WithOpenApi();

        blocks.MapPost("/standard-set", async (
            Guid dayId,
            AddStandardSetBlockCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command with { WorkoutDayId = dayId }, ct);
            return result.ToApiResult(dto => Results.Created(
                $"/api/v1/workout-days/{dayId}", dto));
        })
        .Produces<WorkoutBlockDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add a standard set block (e.g. 3×10 barbell curls, drop sets, TUT)");

        blocks.MapPost("/circuit", async (
            Guid dayId,
            AddCircuitBlockCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command with { WorkoutDayId = dayId }, ct);
            return result.ToApiResult(dto => Results.Created(
                $"/api/v1/workout-days/{dayId}", dto));
        })
        .Produces<WorkoutBlockDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add a circuit/series block (repeated N rounds with rest between)");

        blocks.MapPost("/cardio", async (
            Guid dayId,
            AddCardioBlockCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command with { WorkoutDayId = dayId }, ct);
            return result.ToApiResult(dto => Results.Created(
                $"/api/v1/workout-days/{dayId}", dto));
        })
        .Produces<WorkoutBlockDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add a cardio block (active rest days, cardio finishers)");
    }
}

// ─── Request-only DTOs ────────────────────────────────────────────────────────
public sealed record AddWeekTemplateRequest(int WeekNumber);