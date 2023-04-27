using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.Estimates
{
    public class GetEstimateByIdWithLineItemsClientAndAttachments : Specification<Estimate>, ISingleResultSpecification
    {
        public GetEstimateByIdWithLineItemsClientAndAttachments(string id)
        {
            Query.Where(x => x.Id == id)
                 .Include(x => x.LineItems)
                 .ThenInclude(x=>x.Attachments)
                 .Include(x=>x.Client)
                 .Include(x=>x.Attachments)
                 .AsNoTracking();
        }
    }
}
