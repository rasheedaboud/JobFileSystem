using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using Core.Features.JobFiles.Events;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.JobFiles.Commands
{
    public record DeleteJobFile(string Id) : IRequest<bool>
    {

    }
    public class DeleteJobFileHandler : IRequestHandler<DeleteJobFile, bool>
    {
        private readonly IRepository<JobFile> _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public DeleteJobFileHandler(IRepository<JobFile> context, 
                                    IMapper mapper,
                                    IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<bool> Handle(DeleteJobFile request, CancellationToken cancellationToken)
        {
            var entity = await _context.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null) throw new NotFoundException();
            var paths = entity.Attachments.Select(x => x.BlobPath).ToArray();

            entity.IsDeleted = true;

            await _context.UpdateAsync(entity, cancellationToken);
            await _mediator.Publish(new JobFileDeleted(paths), cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
