using Ardalis.Result;
using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.JobFiles;
using MediatR;

namespace Core.Features.JobFiles.Commands
{
    public record UpdateJobFile(JobFileDto Jobfile) : IRequest<JobFileDto>
    {

    }
    public class UpdateFileHandler : IRequestHandler<UpdateJobFile, JobFileDto>
    {
        private readonly IRepository<JobFile> _context;
        private readonly IRepository<Contact> _contacts;

        public UpdateFileHandler(IRepository<JobFile> context, IRepository<Contact> _contacts)
        {
            _context = context;
            this._contacts = _contacts;
        }

        public async Task<JobFileDto> Handle(UpdateJobFile request, CancellationToken cancellationToken)
        {
            var entity = await _context.GetByIdAsync(request.Jobfile.Id);

            if (entity is null) throw new NotFoundException();

            var contact =
                (await _contacts.ListAsync()).FirstOrDefault(x => x.Name == request.Jobfile.ContactCompany);

            if (contact is null) throw new NotFoundException($"No contact found with name:{request.Jobfile.ContactCompany}");


            var removedFiles = new HashSet<Attachment>();
            var addedFiles = new HashSet<AttachmentDto>();

            foreach (var file in request.Jobfile.Attachments)
            {
                if(!entity.Attachments.Any(x=>x.FileName==file.FileName))
                    addedFiles.Add(file);
            }

            foreach (var attachment in entity.Attachments)
            {
                if (!request.Jobfile.Attachments.Any(x => x.FileName == attachment.FileName))
                    removedFiles.Add(attachment);
            }

            await _context.UpdateAsync(entity.UpdateJobFile(request.Jobfile),                                                          
                                                            cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<JobFileDto>.Success(Mappings.MapJobFileToDto(entity));
        }
    }
}
