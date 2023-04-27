using Core.Entities;
using Core.Specifications.Estimates;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Estimates.Queries
{
    public record GetEstimatesQuery : IRequest<List<EstimateDto>>
    {
    }
    public class GetEstimatesHandler : IRequestHandler<GetEstimatesQuery, List<EstimateDto>>
    {
        private readonly IRepository<Estimate> _context;
        private readonly IAzureBlobService _azureBlobService;

        public GetEstimatesHandler(IRepository<Estimate> context,
                                   IAzureBlobService azureBlobService)
        {
            _context = context;
            _azureBlobService = azureBlobService;
        }

        public async Task<List<EstimateDto>> Handle(GetEstimatesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _context.ListAsync(new GetEstimatesWithClients(), cancellationToken);


            var result = EstimateMapper.MapEstimatesToDtos(entities);

            foreach (var dto in result)
            {
                foreach (var item in dto.Attachments)
                {
                    item.Link = await _azureBlobService.DownloadAsURI(item.BlobPath);

                }
                foreach (var line in dto.LineItems)
                {
                    foreach (var item in line.Attachments)
                    {
                        item.Link = await _azureBlobService.DownloadAsURI(item.BlobPath);
                    }

                }
            }



            return result;
        }
    }
}
