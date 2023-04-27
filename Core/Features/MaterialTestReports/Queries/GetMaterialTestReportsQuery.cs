using Core.Entities;
using Core.Specifications.MaterialTestReports;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared.DTOs;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.MaterialTestReports.Queries
{
    public record GetMaterialTestReportsQuery : IRequest<List<MaterialTestReportDto>>
    {
    }
    public class GetMaterialTestReportsHandler : IRequestHandler<GetMaterialTestReportsQuery, List<MaterialTestReportDto>>
    {
        private readonly IRepository<MaterialTestReport> _context;
        private readonly IAzureBlobService _azureBlobService;

        public GetMaterialTestReportsHandler(IRepository<MaterialTestReport> context,
                                   IAzureBlobService azureBlobService)
        {
            _context = context;
            _azureBlobService = azureBlobService;
        }

        public async Task<List<MaterialTestReportDto>> Handle(GetMaterialTestReportsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _context.ListAsync(new GetMaterialTestReportsWithAttachments(), cancellationToken);


            var result = MaterialTestReportMapper.MapMaterialTestReportsToDtos(entities);

            foreach (var dto in result)
            {
                foreach (var item in dto.Attachments)
                {
                    item.Link = await _azureBlobService.DownloadAsURI(item.BlobPath);

                }
            }

            return result;
        }
    }
}
