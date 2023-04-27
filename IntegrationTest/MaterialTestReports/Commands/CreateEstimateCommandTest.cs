using Core.Entities;
using Core.Features.Contacts.Commands;
using Core.Features.MaterialTestReports.Commands;
using FluentAssertions;
using NUnit.Framework;

namespace IntegrationTests;

using static Testing;


public class CreateMaterialTestReportCommandTest : TestBase
{
    [Test]
    public async Task ShouldCreateMaterialTestReport()
    {
        var userId = await RunAsDefaultUserAsync();


        var MaterialTestReport = await SendAsync(new CreateMaterialTestReportCommand(new()
        {
            HeatNumber = "123465",
            MaterialType = "A106",
            MaterialGrade = "B",
            MaterialForm = "Pipe",
            Length = 20,
            Width = 0,
            Diameter = 2,
            Thickness = 0.25m,
            Description = "2\" SCH40 A106 GR.B PIPE SMLS PBE",
            Location = "",
            Quantity = 50,
            UnitOfMeasure = "FT",
            Vendor = "TENARIS",
        }));


        var item = await FindAsync<MaterialTestReport>(MaterialTestReport.Id);

        item.Should().NotBeNull();
        item.HeatNumber.Should().Be(MaterialTestReport.HeatNumber);
        item.MaterialType.Should().Be(MaterialTestReport.MaterialType);
        item.MaterialGrade.Should().Be(MaterialTestReport.MaterialGrade);
        item.MaterialForm.Name.Should().Be(MaterialTestReport.MaterialForm);
        item.Length.Should().Be(MaterialTestReport.Length);
        item.Width.Should().Be(MaterialTestReport.Width);
        item.Diameter.Should().Be(MaterialTestReport.Diameter);
        item.Thickness.Should().Be(MaterialTestReport.Thickness);
        item.Description.Should().Be(MaterialTestReport.Description);
        item.Location.Should().Be(MaterialTestReport.Location);
        item.Quantity.Should().Be(MaterialTestReport.Quantity);
        item.UnitOfMeasure.Should().Be(MaterialTestReport.UnitOfMeasure);
        item.Vendor.Should().Be(MaterialTestReport.Vendor);

        item.CreatedBy.Should().Be(userId);
        item.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().BeEmpty();
        item.LastModified.Should().BeNull();

    }
}

