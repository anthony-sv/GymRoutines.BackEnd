using Application.Features.TrainingPlans.DTOs;
using FluentValidation;

namespace Application.Features.TrainingPlans.Validators;

public sealed class AddCircuitBlockCommandValidator : AbstractValidator<AddCircuitBlockCommand>
{
    public AddCircuitBlockCommandValidator()
    {
        RuleFor(x => x.WorkoutDayId).NotEmpty();
        RuleFor(x => x.Rounds).InclusiveBetween(1, 50);
        RuleFor(x => x.RestBetweenRoundsSeconds).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Exercises).NotEmpty().WithMessage("A circuit must have at least one exercise.");
    }
}