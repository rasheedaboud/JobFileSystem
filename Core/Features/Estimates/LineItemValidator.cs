using FluentValidation;
using JobFileSystem.Shared.LineItems;

namespace Core.Features.Estimates
{
    public class LineItemValidator : AbstractValidator<LineItemDto>
    {
        public LineItemValidator()
        {
            RuleFor(c => c.Qty).GreaterThan(0).WithMessage("*Qty cannot be zero.");
            RuleFor(c => c.EstimatedUnitPrice).GreaterThan(0).WithMessage("*Estimated Unit Price cannot be zero.");
            RuleFor(c => c.Delivery).NotEmpty().WithMessage("*Delivery cannot be empty");
            RuleFor(c => c.Description).NotEmpty().WithMessage("*Description cannot be empty");
            RuleFor(c => c.UnitOfMeasure).NotEmpty().WithMessage("*Unit cannot be empty");
        }
    }
}
