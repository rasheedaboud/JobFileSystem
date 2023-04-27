using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using Core.Features.Contacts.Exceptions;
using Core.Specifications.Contacts;
using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.JobFiles;
using MediatR;

namespace Core.Features.Contacts.Commands
{
    public record UpdateContactCommand(ContactDto Contact) : IRequest<ContactDto>
    {

    }
    public class UpdateJobFileHandler : IRequestHandler<UpdateContactCommand, ContactDto>
    {
        private readonly IRepository<Contact> _context;
        public UpdateJobFileHandler(IRepository<Contact> context)
        {
            _context = context;
        }

        public async Task<ContactDto> Handle(UpdateContactCommand req, CancellationToken cancellationToken)
        {

            var entity = await _context.GetBySpecAsync(new GetContactById(req.Contact.Id));

            if (entity is null)
                throw new ContactNotFoundException();

            var result = entity.Update(req.Contact.Name,
                                       req.Contact.Company,
                                       req.Contact.ContactMethod,
                                       req.Contact.ContactType,
                                       req.Contact.Email,
                                       req.Contact.Phone);
         
            await _context.UpdateAsync(result, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<ContactDto>.Success(ContactMapper.MapContactToDto(result));
        }
    }
}
