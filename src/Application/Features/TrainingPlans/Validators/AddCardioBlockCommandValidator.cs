using Application.Features.TrainingPlans.DTOs;
using FluentValidation;

namespace Application.Features.TrainingPlans.Validators;

public sealed class AddCardioBlockCommandValidator : AbstractValidator<AddCardioBlockCommand>
{
    public AddCardioBlockCommandValidator()
    {
        RuleFor(x => x.WorkoutDayId).NotEmpty();
        RuleFor(x => x.DurationMinutes).InclusiveBetween(1, 300);
        RuleFor(x => x.CardioType).IsInEnum();
    }
}