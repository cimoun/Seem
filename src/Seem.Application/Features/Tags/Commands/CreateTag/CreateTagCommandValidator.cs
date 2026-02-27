using FluentValidation;

namespace Seem.Application.Features.Tags.Commands.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name is required.")
            .MaximumLength(100).WithMessage("Tag name must not exceed 100 characters.");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Tag color is required.")
            .Matches(@"^#[0-9A-Fa-f]{6}$").WithMessage("Tag color must be a valid hex color (e.g., #FF5733).");
    }
}
