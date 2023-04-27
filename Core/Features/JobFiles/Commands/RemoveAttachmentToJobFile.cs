using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.JobFiles.Commands
{
    public record RemoveAttachmentToJobFile(string Id,
                                            string FileId) : IRequest<bool>
    {

    }
    public class RemoveAttachmentToJobFileHandler : IRequestHandler<RemoveAttachmentToJobFile, bool>
    {
        private readonly IRepository<JobFile> _context;
        private readonly IMapper _mapper;

        public RemoveAttachmentToJobFileHandler(IRepository<JobFile> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RemoveAttachmentToJobFile request, CancellationToken cancellationToken)
        {
            var entity = await _context.GetByIdAsync(request.Id, cancellationToken);

            if (entity is null) throw new NotFoundException();

            if (!string.IsNullOrWhiteSpace(request.FileId))
            {
                entity.RemoveAttachment(request.FileId);
            }

            await _context.UpdateAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
