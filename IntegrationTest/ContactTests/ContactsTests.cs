using Core.Entities;
using Core.Features.Contacts.Commands;
using Core.Features.Contacts.Queries;
using FluentAssertions;
using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.Enums;
using NUnit.Framework;
using System.Text;

namespace IntegrationTests;

using static Testing;

public class ContactTests : TestBase
{

    public static CreateContactCommand CreateContact() => new CreateContactCommand(new()
    {
        Name = "NWR",
        Company = "NWR",
        ContactMethod = "Email",
        Phone = null,
        ContactType = "Client",
        Email = "raboud@shaw.ca",

    });


    [Test]
    public async Task ShouldCreateContact()
    {
        var userId = await RunAsDefaultUserAsync();

        var contact = await SendAsync(CreateContact());


        var item = await FindAsync<Contact>(contact.Id);

        item.Should().NotBeNull();
        item.Name.Should().Be(contact.Name);
        item.Email.Should().Be(contact.Email);
        item.Phone.Should().BeEmpty();
        item.ContactMethod.Should().Be(ContactMethod.FromName( contact.ContactMethod));
        item.ContactType.Should().Be(ContactType.FromName(contact.ContactType));
        item.CreatedBy.Should().Be(userId);
        item.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().BeEmpty();
        item.LastModified.Should().BeNull();

    }
    [Test]
    public async Task ShouldUpdateContact()
    {
        var userId = await RunAsDefaultUserAsync();

        var contact = await SendAsync(CreateContact());

        contact.Company = "NEW";

        var updatedContact = await SendAsync(new UpdateContactCommand(contact));

        var item = await FindAsync<Contact>(updatedContact.Id);

        item.Should().NotBeNull();
        item.Company.Should().Be(updatedContact.Company);

    }
    [Test]
    public async Task ShouldReturnListOfContacts()
    {
        var userId = await RunAsDefaultUserAsync();

        await SendAsync(CreateContact());
        await SendAsync(CreateContact());
        await SendAsync(CreateContact());


        var result = await SendAsync(new GetContactsQuery());


        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(1);

    }
}