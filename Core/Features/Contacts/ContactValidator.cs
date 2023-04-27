using FluentValidation;
using JobFileSystem.Shared.Contacts;

namespace Core.Features.Contacts
{
    public class ContactValidator : AbstractValidator<ContactDto>
    {
        public ContactValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("*Name cannot be empty");
            RuleFor(c => c.Company).NotEmpty().WithMessage("*Company cannot be empty");
            RuleFor(c => c.ContactType).NotEmpty().WithMessage("*Contact type cannot be empty");
            RuleFor(c => c.ContactMethod).NotEmpty().WithMessage("*Contact method cannot be empty");

        }
    }
}
