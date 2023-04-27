using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.MaterialTestReports.Commands
{
    public record RemoveAttachmentFromMaterialTestReport(string MaterialTestReportId,
                                               string fileId) : IRequest<bool>
    {

    }
    public class RemoveAttachmentFromMaterialTestReportHandler : IRequestHandler<RemoveAttachmentFromMaterialTestReport, bool>
    {
        private readonly IRepository<MaterialTestReport> _context;
        private readonly IMapper _mapper;

        public RemoveAttachmentFromMaterialTestReportHandler(IRepository<MaterialTestReport> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RemoveAttachmentFromMaterialTestReport request, CancellationToken cancellationToken)
        {
            var entity = await _context.GetByIdAsync(request.MaterialTestReportId, cancellationToken);

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
