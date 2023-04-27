using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.MaterialTestReports.Commands
{
    public record AddAttachmentToMaterialTestReport(string Id,
                                         string FileName = null,
                                         Stream File = null) : IRequest<bool>
    {

    }
    public class AddAttachmentToMaterialTestReportHandler : IRequestHandler<AddAttachmentToMaterialTestReport, bool>
    {
        private readonly IRepository<MaterialTestReport> _context;


        public AddAttachmentToMaterialTestReportHandler(IRepository<MaterialTestReport> context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AddAttachmentToMaterialTestReport request, CancellationToken cancellationToken)
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
