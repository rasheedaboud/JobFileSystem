using Ardalis.Result;
using Core.Entities;
using Core.Features.MaterialTestReports.Exceptions;
using Core.Specifications.MaterialTestReports;
using JobFileSystem.Shared.DTOs;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.MaterialTestReports.Commands
{
    public record UpdateMaterialTestReportCommand(MaterialTestReportDto MaterialTestReport) : IRequest<MaterialTestReportDto>
    {

    }
    public class UpdateJobFileHandler : IRequestHandler<UpdateMaterialTestReportCommand, MaterialTestReportDto>
    {
        private readonly IRepository<MaterialTestReport> _context;
        public UpdateJobFileHandler(IRepository<MaterialTestReport> context)
        {
            _context = context;
        }

        public async Task<MaterialTestReportDto> Handle(UpdateMaterialTestReportCommand req, CancellationToken cancellationToken)
        {

            var entity = await _context.GetBySpecAsync(new GetMaterialTestReportByIdWithAttachments(req.MaterialTestReport.Id));

            if (entity is null)
                throw new MaterialTestReportNotFoundException();

            var result = entity.Update(req.MaterialTestReport);

            await _context.UpdateAsync(result, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<MaterialTestReportDto>.Success(MaterialTestReportMapper.MapMaterialTestReportToDto(result));
        }
    }
}
