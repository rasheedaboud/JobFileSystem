using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Estimates.Commands
{
    public record RemoveAttachmentFromLineItem(string estimateId,
                                               string lineItemId,
                                               string fileId) : IRequest<bool>
    {

    }
    public class RemoveAttachmentFromLineItemHandler : IRequestHandler<RemoveAttachmentFromLineItem, bool>
    {
        private readonly IRepository<Estimate> _context;
        private readonly IMapper _mapper;

        public RemoveAttachmentFromLineItemHandler(IRepository<Estimate> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RemoveAttachmentFromLineItem request, CancellationToken cancellationToken)
        {
            var entity = await _context.GetByIdAsync(request.estimateId, cancellationToken);

            if (entity is null) throw new NotFoundException();

            if (!string.IsNullOrWhiteSpace(request.fileId))
            {
                entity.RemoveLineItemAttachment(request.lineItemId,request.fileId);
            }

            await _context.UpdateAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
