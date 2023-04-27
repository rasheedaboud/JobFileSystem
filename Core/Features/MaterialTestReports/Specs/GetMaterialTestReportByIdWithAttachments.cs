using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.MaterialTestReports
{
    public class GetMaterialTestReportByIdWithAttachments : Specification<MaterialTestReport>, ISingleResultSpecification
    {
        public GetMaterialTestReportByIdWithAttachments(string id)
        {
            Query.Where(x => x.Id == id).Include(x=>x.Attachments).AsNoTracking();
        }
    }
}
