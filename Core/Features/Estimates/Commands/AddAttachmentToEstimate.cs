using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Estimates.Commands
{
    public record AddAttachmentToEstimate(string Id,
                                         string FileName = null,
                                         Stream File = null) : IRequest<bool>
    {

    }
    public class AddAttachmentToEstimateHandler : IRequestHandler<AddAttachmentToEstimate, bool>
    {
        private readonly IRepository<Estimate> _context;
        private readonly IMapper _mapper;

        public AddAttachmentToEstimateHandler(IRepository<Estimate> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddAttachmentToEstimate request, CancellationToken cancellationToken)
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
