using Core.Entities;
using Core.Specifications.JobFiles;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.JobFiles;
using MediatR;

namespace Core.Features.JobFiles.Queries
{
    public record GetJobFilesQuery : IRequest<List<JobFileDto>>
    {
    }
    public class GetJobFilesHandler : IRequestHandler<GetJobFilesQuery, List<JobFileDto>>
    {
        private readonly IRepository<JobFile> _context;

        public GetJobFilesHandler(IRepository<JobFile> context)
        {
            _context = context;
        }

        public async Task<List<JobFileDto>> Handle(GetJobFilesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _context.ListAsync(new GetJobFilesWithEstimateContactAndAttachments(),cancellationToken);

            return Mappings.MapJobFilesToDto(entities);
        }


    }
}
