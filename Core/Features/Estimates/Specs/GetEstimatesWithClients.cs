using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.Estimates
{
    public class GetEstimatesWithClients : Specification<Estimate>
    {
        public GetEstimatesWithClients()
        {
            Query.Include(x => x.Client).Include(x=>x.Attachments).Include(x=>x.LineItems).ThenInclude(x=>x.Attachments).AsNoTracking();
        }
    }
}
