using Ardalis.Result;
using Core.Entities;
using JobFileSystem.Shared.DTOs;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.MaterialTestReports.Commands
{
    public record CreateMaterialTestReportCommand(MaterialTestReportDto MaterialTestReport) : IRequest<MaterialTestReportDto>
    {

    }
    public class CreateJobFileHandler : IRequestHandler<CreateMaterialTestReportCommand, MaterialTestReportDto>
    {
        private readonly IRepository<MaterialTestReport> _context;
        public CreateJobFileHandler(IRepository<MaterialTestReport> context)
        {
            _context = context;
        }

        public async Task<MaterialTestReportDto> Handle(CreateMaterialTestReportCommand req, CancellationToken cancellationToken)
        {

            var entity = new MaterialTestReport(req.MaterialTestReport);

            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<MaterialTestReportDto>.Success(MaterialTestReportMapper.MapMaterialTestReportToDto(entity));
        }
    }
}
