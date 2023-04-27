using FluentValidation;
using JobFileSystem.Shared.DTOs;
using JobFileSystem.Shared.Enums;

namespace Core.Features.MaterialTestReports
{
    public class MaterialTestReportValidator : AbstractValidator<MaterialTestReportDto>
    {
        public MaterialTestReportValidator()
        {
            RuleFor(c => c.Thickness).GreaterThanOrEqualTo(0).WithMessage("*Invalid Thickness.");
            RuleFor(c => c.Width).GreaterThanOrEqualTo(0).WithMessage("*Invalid Width.");
            RuleFor(c => c.Diameter).GreaterThanOrEqualTo(0).WithMessage("*Invalid Diameter.");
            RuleFor(c => c.Length).GreaterThanOrEqualTo(0).WithMessage("*Invalid Length.");
            RuleFor(c => c.Quantity).GreaterThan(0).WithMessage("*Invalid Quantity.");

            RuleFor(c => c.UnitOfMeasure).NotEmpty().WithMessage("*Invalid Unit.");
            RuleFor(c => c.UnitOfMeasure).MaximumLength(25).WithMessage("*Unit too long.");


            RuleFor(c => c.Description).NotEmpty().WithMessage("*Invalid Description.");
            RuleFor(c => c.Description).MaximumLength(100).WithMessage("*Description too long.");
            
            
            
            RuleFor(c => c.HeatNumber).NotEmpty().WithMessage("*Invalid Heat No.");
            RuleFor(c => c.HeatNumber).MaximumLength(100).WithMessage("*Heat No. too long.");
            
            
            
            RuleFor(c => c.Location).NotEmpty().WithMessage("*Invalid Location");
            RuleFor(c => c.Location).MaximumLength(100).WithMessage("*Location too long.");

            RuleFor(c => c.MaterialForm).Must(BeValidMaterialForm).WithMessage("*Invalid material Form.");

            RuleFor(c => c.MaterialGrade).NotEmpty().WithMessage("*Invalid Grade");
            RuleFor(c => c.MaterialGrade).MaximumLength(50).WithMessage("*Grade too long.");

            RuleFor(c => c.MaterialType).NotEmpty().WithMessage("*Invalid Type Grade");
            RuleFor(c => c.MaterialType).MaximumLength(50).WithMessage("*Type too long.");

            RuleFor(c => c.Vendor).NotEmpty().WithMessage("*Invalid Vendor.");
            RuleFor(c => c.Vendor).MaximumLength(100).WithMessage("*Vendor name too long.");
        }

        private bool BeValidMaterialForm(string? name)
        {
            return !string.IsNullOrEmpty(name) && 
                   MaterialForm.TryFromName(name, out var _);
        }
    }
}
