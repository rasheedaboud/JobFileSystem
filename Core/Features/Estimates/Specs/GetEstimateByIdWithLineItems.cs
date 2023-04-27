using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.Estimates
{
    public class GetEstimateByIdWithLineItems : Specification<Estimate>, ISingleResultSpecification
    {
        public GetEstimateByIdWithLineItems(string id)
        {
            Query.Where(x => x.Id == id).Include(x => x.LineItems).AsNoTracking();
        }
    }
}
