using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.JobFiles;
using MediatR;

namespace Core.Features.Contacts.Commands
{
    public record CreateContactCommand(ContactDto Contact) : IRequest<ContactDto>
    {

    }
    public class CreateJobFileHandler : IRequestHandler<CreateContactCommand, ContactDto>
    {
        private readonly IRepository<Contact> _context;
        public CreateJobFileHandler(IRepository<Contact> context)
        {
            _context = context;
        }

        public async Task<ContactDto> Handle(CreateContactCommand req, CancellationToken cancellationToken)
        {

            var entity = new Contact(req.Contact.Name,
                                     req.Contact.Company,
                                     req.Contact.ContactMethod,
                                     req.Contact.ContactType,
                                     req.Contact.Email,
                                     req.Contact.Phone);
         
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<ContactDto>.Success(ContactMapper.MapContactToDto(entity));
        }
    }
}
