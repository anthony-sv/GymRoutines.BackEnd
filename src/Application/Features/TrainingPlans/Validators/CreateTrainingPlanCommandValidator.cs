using Application.Features.TrainingPlans.DTOs;
using FluentValidation;

namespace Application.Features.TrainingPlans.Validators;

public sealed class CreateTrainingPlanCommandValidator : AbstractValidator<CreateTrainingPlanCommand>
{
    public CreateTrainingPlanCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.TotalWeeks).InclusiveBetween(1, 52);
        RuleFor(x => x.StructureType).IsInEnum();
    }
}