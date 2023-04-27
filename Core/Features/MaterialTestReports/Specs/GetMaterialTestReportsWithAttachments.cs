using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.MaterialTestReports
{
    public class GetMaterialTestReportsWithAttachments : Specification<MaterialTestReport>, ISingleResultSpecification
    {
        public GetMaterialTestReportsWithAttachments()
        {
            Query.Include(x => x.Attachments).AsNoTracking();
        }
    }
}
