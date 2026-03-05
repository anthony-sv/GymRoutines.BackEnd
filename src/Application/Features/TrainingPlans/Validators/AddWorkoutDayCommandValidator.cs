using Application.Features.TrainingPlans.DTOs;
using FluentValidation;

namespace Application.Features.TrainingPlans.Validators;

public sealed class AddWorkoutDayCommandValidator : AbstractValidator<AddWorkoutDayCommand>
{
    public AddWorkoutDayCommandValidator()
    {
        RuleFor(x => x.WeekTemplateId).NotEmpty();
        RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6);
        RuleFor(x => x.DayType).IsInEnum();
        RuleFor(x => x.Label).MaximumLength(100);
    }
}