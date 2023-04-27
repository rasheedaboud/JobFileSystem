using FluentValidation;
using JobFileSystem.Shared.Estimates;

namespace Core.Features.Estimates
{
    public class EstimateValidator : AbstractValidator<EstimateDto>
    {
        public EstimateValidator()
        {
            RuleFor(c => c.ShortDescription).MaximumLength(75).WithMessage("*Short Description too long.");
            RuleFor(c => c.ShortDescription).NotEmpty().WithMessage("*Short Description invalid.");
            RuleFor(c => c.LongDescription).NotEmpty().WithMessage("*Long Description cannot be empty.");
            RuleFor(c => c.Client).NotNull().WithMessage("*Client cannot be empty.");
        }
    }
}
