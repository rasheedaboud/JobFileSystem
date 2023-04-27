using Core.Entities;
using Core.Features.Contacts.Commands;
using Core.Features.Estimates.Commands;
using FluentAssertions;
using NUnit.Framework;

namespace IntegrationTests;

using static Testing;


public class CreateLineItemCommandTest : TestBase
{
    [Test]
    public async Task ShouldCreateEstimateWithValidLineItem()
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

        var lineItem = await SendAsync(new CreateLineItemCommand(estimate.Id, new()
        {
            Delivery= "STK",
            Qty = 1.0m,
            UnitOfMeasure = "FT",
            EstimatedUnitPrice = 10.0m,
            Description = "PIPE"
        }));

        lineItem.Should().NotBeNull();
    }
}

