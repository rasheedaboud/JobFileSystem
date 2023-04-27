using Core.Entities;
using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Contacts.Queries
{
    public record GetContactsQuery : IRequest<List<ContactDto>>
    {
    }
    public class GetContactsHandler : IRequestHandler<GetContactsQuery, List<ContactDto>>
    {
        private readonly IRepository<Contact> _context;

        public GetContactsHandler(IRepository<Contact> context)
        {
            _context = context;
        }

        public async Task<List<ContactDto>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _context.ListAsync(cancellationToken);

            return ContactMapper.MapContactsToDtos(entities);
        }


    }
}
