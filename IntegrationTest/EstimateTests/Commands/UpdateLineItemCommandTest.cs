using Core.Entities;
using Core.Features.Contacts.Commands;
using Core.Features.Estimates.Commands;
using Core.Specifications.Estimates;
using FluentAssertions;
using NUnit.Framework;

namespace IntegrationTests;

using static Testing;


public class UpdateLineItemCommandTest : TestBase
{
    [Test]
    public async Task ShouldUpdateEstimateWithValidLineItem()
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

        var item = await FirstAsync<Estimate>(estimate.Id);

        var lineItemUpdated = await SendAsync(new UpdateLineItemCommand(estimate.Id, new()
        {
            Id= item.LineItems.First().Id,
            Delivery = "STK",
            Qty = 1.0m,
            UnitOfMeasure = "FT",
            EstimatedUnitPrice = 10.0m,
            Description = "PLATE"
        }));

        lineItem.Should().NotBeNull();
        lineItemUpdated.Should().NotBeNull();
        lineItemUpdated.Delivery.Should().Be("STK");
        lineItemUpdated.Description.Should().Be("PLATE");
    }
}

