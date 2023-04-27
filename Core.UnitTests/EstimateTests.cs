using Core.Entities;
using FluentAssertions;
using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.Enums;
using JobFileSystem.Shared.LineItems;
using NUnit.Framework;
using System;
using System.Linq;

namespace Core.UnitTests
{
    [TestFixture]
    public class EstimateTests
    {
        public static ContactDto Contact => new ContactDto() 
        { 
            Id = Guid.NewGuid().ToString(),
            Name = "scott", 
            Company = "CNRL", 
            ContactMethod = "Email", 
            ContactType = "Client", 
            Email = "ss@google.com", 
            Phone = "780.288.5511" 
        
        };
        public static string ShortDesciption => "Transmitter Fabrication";
        public static string LongDesciption => "Fabricate three (3) transmitters";
        public static DateTime? DeliveryDate => DateTime.UtcNow;
        public static Estimate Estimate => new Estimate(new()
        {
            ShortDescription = ShortDesciption,
            LongDescription = LongDesciption,
            DeliveryDate = DeliveryDate,
            Client = Contact
        });


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldCreateValidEstimate()
        {
            var estimate = Estimate;

            estimate.Should().NotBeNull();
            estimate.Number.Should().NotBeNullOrEmpty();
            estimate.Status.Should().Be(EstimateStatus.New);
            estimate.LongDescription.Should().NotBeNullOrEmpty();
            estimate.LongDescription.Should().Be(LongDesciption);
            estimate.ShortDescription.Should().NotBeNullOrEmpty();
            estimate.ShortDescription.Should().Be(ShortDesciption);
        }
        [Test]
        public void ShouldAddLineItemToEstimate()
        {
            var lineItem = new LineItemDto()
            {
                Description = "2\" SCH40 PIPE SMLS PBE",
                Delivery = "2 DAYS",
                UnitOfMeasure = "FT",
                Qty = 20.0m,
                EstimatedUnitPrice = 10.0m,
            };
            var (estimate, _) = Estimate.AddLineItem(lineItem);

            estimate.LineItems.Should().HaveCount(1);
        }
        [Test]
        public void ShouldUpdateLineItemToEstimate()
        {
            var lineItem = new LineItemDto()
            {
                Description = "2\" SCH40 PIPE SMLS PBE",
                Delivery = "2 DAYS",
                UnitOfMeasure = "FT",
                Qty = 20.0m,
                EstimatedUnitPrice = 10.0m,
            };


            var (estimate, _) = Estimate.AddLineItem(lineItem);

            var updatedLineItem = lineItem;
            updatedLineItem.Id = estimate.LineItems.First().Id;
            updatedLineItem.Qty = 20.0m;
            updatedLineItem.Description = "NIPPLE A105N 6IN LONG";
            updatedLineItem.Delivery = "4 DAYS";
            updatedLineItem.EstimatedUnitPrice = 20.0m;
            updatedLineItem.UnitOfMeasure = "IN";

            var (result, _) = estimate.UpdateLineItem(updatedLineItem);

            result.LineItems.First().Qty.Should().Be(updatedLineItem.Qty);
            result.LineItems.First().Description.Should().Be(updatedLineItem.Description);
            result.LineItems.First().Delivery.Should().Be(updatedLineItem.Delivery);
            result.LineItems.First().EstimatedUnitPrice.Should().Be(updatedLineItem.EstimatedUnitPrice);
            result.LineItems.First().UnitOfMeasure.Should().Be(updatedLineItem.UnitOfMeasure);
        }
        [Test]
        public void ShouldCalculateTotalsProperlyToEstimate()
        {
            var lineItem = new LineItemDto()
            {
                Description = "2\" SCH40 PIPE SMLS PBE",
                Delivery = "2 DAYS",
                UnitOfMeasure = "FT",
                Qty = 20.0m,
                EstimatedUnitPrice = 10.0m,
            };
            var (estimate, _) = Estimate.AddLineItem(lineItem);
            //12
            var gst = 12;

            //240
            var subtotal = 240;
            //252
            var total = subtotal + gst;

            estimate.LineItems.Should().HaveCount(1);
            estimate.Total.Should().Be(total);
            estimate.Gst.Should().Be(gst);
            estimate.Subtotal.Should().Be(subtotal);
        }
    }
}