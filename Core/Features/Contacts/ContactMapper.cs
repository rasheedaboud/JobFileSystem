using Core.Entities;
using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.Enums;

namespace Core.Features.Contacts
{
    public static class ContactMapper
    {
        public static List<ContactDto> MapContactsToDtos(List<Contact> entities)
        {
            return entities.Select(contact => new ContactDto()
            {
                Id= contact.Id,
                Company = contact.Company,
                ContactType = contact.ContactType.Name,
                Name = contact.Name,
                ContactMethod = contact.ContactMethod.Name,
                Email = contact.Email,
                Phone = contact.Phone,
            }).ToList();
        }

        public static ContactDto MapContactToDto(Contact contact) => new()
        {
            Id = contact.Id,
            Company = contact.Company,
            ContactType = contact.ContactType.Name,
            Name = contact.Name,
            ContactMethod = contact.ContactMethod.Name,
            Email = contact.Email,
            Phone = contact.Phone,
        };
        public static Contact MapDtoToContact(ContactDto contact) => new()
        {
            Id = contact.Id,
            Company = contact.Company,
            ContactType = ContactType.FromName(contact.ContactType),
            Name = contact.Name,
            ContactMethod = ContactMethod.FromName(contact.ContactMethod),
            Email = contact.Email,
            Phone = contact.Phone,
        };
    }
}
