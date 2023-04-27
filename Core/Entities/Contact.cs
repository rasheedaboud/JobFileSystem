using Ardalis.GuardClauses;
using Core.Exceptions;
using JobFileSystem.Shared;
using JobFileSystem.Shared.Enums;
using JobFileSystem.Shared.Interfaces;

namespace Core.Entities
{
    public class Contact : BaseEntity, IAggregateRoot
    {
        public Contact()
        {
        }
        public Contact(string name, string company, string contactMethod, string contactType, string? email = null, string? phone = null)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(name, nameof(company));
            Guard.Against.NullOrWhiteSpace(contactMethod, nameof(contactMethod));
            Guard.Against.NullOrWhiteSpace(contactType, nameof(contactType));

            if (ContactMethod.FromName(contactMethod) == ContactMethod.Email && string.IsNullOrEmpty(email))
                throw new EmailInvalidException();

            if (ContactMethod.FromName(contactMethod) == ContactMethod.Phone && string.IsNullOrEmpty(phone))
                throw new PhoneInvalidException();

            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(phone))
                throw new PhoneAndEmailInvalidException();


            Id = Guid.NewGuid().ToString();
            Name = name;
            Company = company;
            Email = email ?? "";
            Phone = phone ?? "";
            ContactMethod = ContactMethod.FromName(contactMethod);
            ContactType = ContactType.FromName(contactType);
        }
        public Contact Update(string name,
                              string company,
                              string contactMethod,
                              string contactType,
                              string? email = null,
                              string? phone = null) =>
         new()
         {
             Id = Id,
             Name = string.IsNullOrEmpty(name) ? Name : name,
             Company = string.IsNullOrEmpty(company) ? Company : company,
             Email = string.IsNullOrEmpty(email) ? Email : email,
             Phone = string.IsNullOrEmpty(phone) ? Phone : phone,
             ContactMethod = TryGetContactMethod(contactMethod),
             ContactType = TryGetContactType(contactType),
         };

        private ContactType TryGetContactType(string contactType)
        {
            try
            {
                return ContactType.FromName(contactType);
            }
            catch (Exception)
            {
                return ContactType;
            }
        }
        private ContactMethod TryGetContactMethod(string contactMethod)
        {
            try
            {
                return ContactMethod.FromName(contactMethod);
            }
            catch (Exception)
            {
                return ContactMethod;
            }
        }

        public string Name { get; init; }
        public string Company { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public ContactMethod ContactMethod { get; init; }
        public ContactType ContactType { get; init; }
    }
}
