using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.Estimates
{
    public class GetEstimateByIdWithClient : Specification<Estimate>, ISingleResultSpecification
    {
        public GetEstimateByIdWithClient(string id)
        {
            Query.Where(x => x.Id == id).Include(x=>x.Client).AsNoTracking();
        }
    }
}
