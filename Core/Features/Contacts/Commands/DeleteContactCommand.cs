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
    public record DeleteContactCommand(string Id) : IRequest<bool>
    {

    }
    public class DeleteJobFileHandler : IRequestHandler<DeleteContactCommand, bool>
    {
        private readonly IRepository<Contact> _context;
        public DeleteJobFileHandler(IRepository<Contact> context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteContactCommand req, CancellationToken cancellationToken)
        {

            var entity = await _context.GetBySpecAsync(new GetContactById(req.Id));

            if (entity is null)
            {
                throw new ContactNotFoundException();
            }

            entity.IsDeleted = true;
            await _context.UpdateAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
