using Core.Entities;
using Core.Features.Contacts.Commands;
using Core.Features.Estimates.Commands;
using FluentAssertions;
using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.Estimates;
using NUnit.Framework;

namespace IntegrationTests;

using static Testing;


public class UpdateEstimateCommandTest : TestBase
{

    private ContactDto Contact => new()
    {
        Company = "CNRL",
        ContactMethod = "Email",
        ContactType = "Client",
        Email = "raboud@shaw.ca",
        Id = Guid.NewGuid().ToString(),
        Name = "Scott",
        Phone = "780.288.5511"
    };

    private static EstimateDto Estimate(ContactDto contact) => new()
    {
        Client = contact,
        LongDescription = "Fabricate and ship six (6) transmitters",
        ShortDescription = "Transmitter Fabrication"
    };

    [Test]
    public async Task ShouldCreateEstimate()
    {
        var userId = await RunAsDefaultUserAsync();

        var client = await SendAsync(new CreateContactCommand(Contact));

        var estimate = await SendAsync(new CreateEstimateCommand(Estimate(client)));

        var updated = estimate;

        updated.ShortDescription = "NEW DESCRIPTION";
        updated.LongDescription = "NEW LONG ESCRIPTION";
        updated.Client = client;

        var result = await SendAsync(new UpdateEstimateCommand(updated));
        var item = await FindAsync<Estimate>(updated.Id);

        item.Should().NotBeNull();
        item.ShortDescription.Should().Be(updated.ShortDescription);
        item.LongDescription.Should().Be(updated.LongDescription);
        item.CreatedBy.Should().Be(userId);
        item.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().NotBeEmpty();
        item.LastModified.Should().NotBeNull();

    }
}

