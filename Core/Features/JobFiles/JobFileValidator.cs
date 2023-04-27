using FluentValidation;
using JobFileSystem.Shared.JobFiles;

namespace Core.Features.JobFiles
{

    public class JobFileValidator : AbstractValidator<JobFileDto>
    {
        public JobFileValidator()
        {

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("*Name is required.")
                .MaximumLength(200).WithMessage("*Name must not exceed 200 characters.");
            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("*Description is required.");
            RuleFor(v => v.ContactCompany)
                .NotEmpty().WithMessage("*Client is required.");

        }
    }
}
