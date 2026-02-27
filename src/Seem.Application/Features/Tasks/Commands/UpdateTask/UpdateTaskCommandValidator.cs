using FluentValidation;

namespace Seem.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Task ID is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .MaximumLength(500).WithMessage("Task title must not exceed 500 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(10000).WithMessage("Description must not exceed 10000 characters.");
    }
}
