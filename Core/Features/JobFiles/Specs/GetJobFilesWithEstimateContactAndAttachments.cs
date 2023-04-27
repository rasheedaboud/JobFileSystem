using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.JobFiles
{
    public class GetJobFilesWithEstimateContactAndAttachments : Specification<JobFile>, ISingleResultSpecification
    {
        public GetJobFilesWithEstimateContactAndAttachments()
        {
            Query.Include(x=>x.Contact)
                 .Include(x=>x.Attachments)
                 .Include(x=>x.Estimate).AsNoTracking();
        }
    }
}
