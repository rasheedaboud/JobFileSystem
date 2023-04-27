using Core.Entities;
using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Contacts.Queries
{
    public record GetContactByIdQuery(string id) : IRequest<ContactDto>
    {
    }
    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto>
    {
        private readonly IRepository<Contact> _context;

        public GetContactByIdQueryHandler(IRepository<Contact> context)
        {
            _context = context;
        }

        public async Task<ContactDto> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.GetByIdAsync(request.id,cancellationToken);

            if(entity is null) throw new DirectoryNotFoundException();

            return ContactMapper.MapContactToDto(entity);
        }


    }
}
