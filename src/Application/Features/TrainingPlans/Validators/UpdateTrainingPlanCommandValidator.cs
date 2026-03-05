using Application.Features.TrainingPlans.DTOs;
using FluentValidation;

namespace Application.Features.TrainingPlans.Validators;

public sealed class UpdateTrainingPlanCommandValidator : AbstractValidator<UpdateTrainingPlanCommand>
{
    public UpdateTrainingPlanCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(200);
    }
}