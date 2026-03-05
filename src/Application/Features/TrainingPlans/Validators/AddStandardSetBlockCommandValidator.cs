using Application.Features.TrainingPlans.DTOs;
using FluentValidation;

namespace Application.Features.TrainingPlans.Validators;

public sealed class AddStandardSetBlockCommandValidator : AbstractValidator<AddStandardSetBlockCommand>
{
    public AddStandardSetBlockCommandValidator()
    {
        RuleFor(x => x.WorkoutDayId).NotEmpty();
        RuleFor(x => x.ExerciseId).NotEmpty();
        RuleFor(x => x.Sets).InclusiveBetween(1, 20);
        RuleFor(x => x.RestSeconds).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MinReps).GreaterThan(0).When(x => x.MinReps.HasValue);
        RuleFor(x => x.MaxReps).GreaterThanOrEqualTo(x => x.MinReps ?? 0)
            .When(x => x.MaxReps.HasValue && x.MinReps.HasValue);
    }
}