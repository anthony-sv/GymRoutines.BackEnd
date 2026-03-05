using Application.Features.Exercises.DTOs;
using FluentValidation;
using Domain.Enums;

namespace Application.Features.Exercises.Validators;

public sealed class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    public CreateExerciseCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(200);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.PrimaryMuscles).NotNull();
        RuleFor(x => x.SecondaryMuscles).NotNull();
        When(x => x.Type == ExerciseType.Cardio, () =>
            RuleFor(x => x.DefaultCardioType).NotNull().WithMessage("Cardio type is required for cardio exercises."));
    }
}