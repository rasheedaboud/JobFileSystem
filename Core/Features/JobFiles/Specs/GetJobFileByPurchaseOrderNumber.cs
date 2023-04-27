using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.JobFiles
{
    public class GetJobFileByPurchaseOrderNumber : Specification<JobFile>, ISingleResultSpecification
    {
        public GetJobFileByPurchaseOrderNumber(string purchaseOrderNumber)
        {
            Query.Where(x => x.PurchaseOrderNumber == purchaseOrderNumber).AsNoTracking();
        }
    }
}
