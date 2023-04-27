using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.Contacts
{
    public class GetContactById : Specification<Contact>, ISingleResultSpecification
    {
        public GetContactById(string id)
        {
            Query.Where(x => x.Id == id).AsNoTracking();
        }
    }
}
