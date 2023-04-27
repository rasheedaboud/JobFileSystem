using Core.Entities;
using Core.Specifications.MaterialTestReports;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared.DTOs;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.MaterialTestReports.Queries
{
    public record GetMaterialTestReportByIdQuery(string MaterialTestReportId) : IRequest<MaterialTestReportDto>
    {
    }
    public class GetMaterialTestReportByIdHandler : IRequestHandler<GetMaterialTestReportByIdQuery, MaterialTestReportDto>
    {
        private readonly IRepository<MaterialTestReport> _context;
        private readonly IAzureBlobService _azureBlobService;

        public GetMaterialTestReportByIdHandler(IRepository<MaterialTestReport> context,
                                      IAzureBlobService azureBlobService)
        {
            _context = context;
            _azureBlobService = azureBlobService;
        }

        public async Task<MaterialTestReportDto> Handle(GetMaterialTestReportByIdQuery req, CancellationToken cancellationToken)
        {
            var entity = await _context.GetBySpecAsync(new GetMaterialTestReportByIdWithAttachments(req.MaterialTestReportId), cancellationToken);

            var result = MaterialTestReportMapper.MapMaterialTestReportToDto(entity);

            foreach (var item in result.Attachments)
            {
                item.Link = await _azureBlobService.DownloadAsURI(item.BlobPath);

            }
            return result;
        }
    }
}
