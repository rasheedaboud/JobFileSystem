using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.JobFiles.Commands
{
    public record AddAttachmentToJobFile(string Id,
                                         string FileName = null,
                                         Stream File = null) : IRequest<bool>
    {

    }
    public class AddAttachmentToJobFileHandler : IRequestHandler<AddAttachmentToJobFile, bool>
    {
        private readonly IRepository<JobFile> _context;
        private readonly IMapper _mapper;

        public AddAttachmentToJobFileHandler(IRepository<JobFile> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddAttachmentToJobFile request, CancellationToken cancellationToken)
        {
            var entity = await _context.GetByIdAsync(request.Id, cancellationToken);

            if (entity is null) throw new NotFoundException();

            if (!string.IsNullOrWhiteSpace(request.FileName) && request.File != null && request.File.Length > 0)
            {
                entity.AddAttachment(request.FileName, request.File);
            }

            await _context.UpdateAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
