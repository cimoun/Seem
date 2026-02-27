using FluentValidation;

namespace Seem.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(200).WithMessage("Project name must not exceed 200 characters.");

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Project key is required.")
            .MinimumLength(1).WithMessage("Project key must be at least 1 character.")
            .MaximumLength(5).WithMessage("Project key must not exceed 5 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Description must not exceed 5000 characters.");
    }
}
