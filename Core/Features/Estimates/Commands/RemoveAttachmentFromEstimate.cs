using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Estimates.Commands
{
    public record RemoveAttachmentFromEstimate(string estimateId,
                                               string fileId) : IRequest<bool>
    {

    }
    public class RemoveAttachmentFromEstimateHandler : IRequestHandler<RemoveAttachmentFromEstimate, bool>
    {
        private readonly IRepository<Estimate> _context;
        private readonly IMapper _mapper;

        public RemoveAttachmentFromEstimateHandler(IRepository<Estimate> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RemoveAttachmentFromEstimate request, CancellationToken cancellationToken)
        {
            var entity = await _context.GetByIdAsync(request.estimateId, cancellationToken);

            if (entity is null) throw new NotFoundException();

            if (!string.IsNullOrWhiteSpace(request.fileId))
            {
                entity.RemoveAttachment(request.fileId);
            }

            await _context.UpdateAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
