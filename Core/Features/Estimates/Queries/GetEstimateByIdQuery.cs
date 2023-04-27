using Core.Entities;
using Core.Specifications.Estimates;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Estimates.Queries
{
    public record GetEstimateByIdQuery(string estimateId) : IRequest<EstimateDto>
    {
    }
    public class GetEstimateByIdHandler : IRequestHandler<GetEstimateByIdQuery, EstimateDto>
    {
        private readonly IRepository<Estimate> _context;
        private readonly IAzureBlobService _azureBlobService;

        public GetEstimateByIdHandler(IRepository<Estimate> context, 
                                      IAzureBlobService azureBlobService)
        {
            _context = context;
            _azureBlobService = azureBlobService;
        }

        public async Task<EstimateDto> Handle(GetEstimateByIdQuery req, CancellationToken cancellationToken)
        {
            var entity = await _context.GetBySpecAsync(new GetEstimateByIdWithLineItemsClientAndAttachments(req.estimateId), cancellationToken);

            var result = EstimateMapper.MapEstimateToDto(entity);

            foreach (var item in result.Attachments)
            {
                item.Link = await _azureBlobService.DownloadAsURI(item.BlobPath);

            }
            foreach (var line in result.LineItems)
            {
                foreach (var item in line.Attachments)
                {
                    item.Link = await _azureBlobService.DownloadAsURI(item.BlobPath);
                }
               
            }


            return result;
        }
    }
}
