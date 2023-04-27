using Core.Entities;
using Core.Features.Contacts.Commands;
using Core.Features.Estimates.Commands;
using FluentAssertions;
using NUnit.Framework;

namespace IntegrationTests;

using static Testing;


public class CreateEstimateCommandTest : TestBase
{
    [Test]
    public async Task ShouldCreateEstimate()
    {
        var userId = await RunAsDefaultUserAsync();

        var client = await SendAsync(new CreateContactCommand(new()
        {
            Company = "CNRL",
            ContactMethod = "Email",
            ContactType = "Client",
            Email = "raboud@shaw.ca",
            Id = Guid.NewGuid().ToString(),
            Name = "Scott",
            Phone = "780.288.5511"
        }));

        var estimate = await SendAsync(new CreateEstimateCommand(new()
        {
            Client = client,
            LongDescription = "Fabricate and ship six (6) transmitters",
            ShortDescription = "Transmitter Fabrication"
        }));


        var item = await FindAsync<Estimate>(estimate.Id);

        item.Should().NotBeNull();
        item.ShortDescription.Should().Be(estimate.ShortDescription);
        item.LongDescription.Should().Be(estimate.LongDescription);
        item.CreatedBy.Should().Be(userId);
        item.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().BeEmpty();
        item.LastModified.Should().BeNull();

    }
}

