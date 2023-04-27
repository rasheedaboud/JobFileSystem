using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.JobFiles;
using MediatR;

namespace Core.Features.JobFiles.Commands
{
    public record CreateJobFile(JobFileDto JobFile) : IRequest<JobFileDto>
    {

    }
    public class CreateJobFileHandler : IRequestHandler<CreateJobFile, JobFileDto>
    {
        private readonly IRepository<JobFile> _context;
        private readonly IRepository<Contact> _contacts;

        public CreateJobFileHandler(IRepository<JobFile> context, IRepository<Contact> contacts)
        {
            _context = context;
            _contacts = contacts;
        }

        public async Task<JobFileDto> Handle(CreateJobFile request, CancellationToken cancellationToken)
        {

            var contact = 
                (await _contacts.ListAsync()).FirstOrDefault(x=>x.Name == request.JobFile.ContactCompany);

            if (contact is null) throw new NotFoundException($"No contact found with name:{request.JobFile.ContactCompany}");

            var count = (await _context.CountAsync()) + 1;

            var entity = new JobFile(count, request.JobFile);
          

            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<JobFileDto>.Success(Mappings.MapJobFileToDto(entity));
        }
    }
}
