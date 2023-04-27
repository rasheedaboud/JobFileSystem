using Core.Entities;
using Core.Features.MaterialTestReports.Commands;
using FluentAssertions;
using JobFileSystem.Shared.DTOs;
using NUnit.Framework;

namespace IntegrationTests;

using static Testing;


public class UpdateMaterialTestReportCommandTest : TestBase
{

    private static MaterialTestReportDto MTR => new()
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
    };
    private static MaterialTestReportDto Update(MaterialTestReportDto dto)
    {
        dto.HeatNumber = "123TTT465";
        dto.MaterialType = "TTT";
        dto.MaterialGrade = "T";
        dto.MaterialForm = "Pipe";
        dto.Length = 5;
        dto.Width = 5;
        dto.Diameter = 5;
        dto.Thickness = 5;
        dto.Description = "2\" TTTT";
        dto.Location = "TTTT";
        dto.Quantity = 5;
        dto.UnitOfMeasure = "TTT";
        dto.Vendor = "TTTT";
        return dto;
    }

    [Test]
    public async Task ShouldCreateEstimate()
    {
        var userId = await RunAsDefaultUserAsync();

        var mtr = await SendAsync(new CreateMaterialTestReportCommand(MTR));

        var updated = Update(mtr);



        var result = await SendAsync(new UpdateMaterialTestReportCommand(updated));
        var item = await FindAsync<MaterialTestReport>(updated.Id);

        item.Should().NotBeNull();
        item.HeatNumber.Should().Be(updated.HeatNumber);
        item.MaterialType.Should().Be(updated.MaterialType);
        item.MaterialGrade.Should().Be(updated.MaterialGrade);
        item.MaterialForm.Name.Should().Be(updated.MaterialForm);
        item.Length.Should().Be(updated.Length);
        item.Width.Should().Be(updated.Width);
        item.Diameter.Should().Be(updated.Diameter);
        item.Thickness.Should().Be(updated.Thickness);
        item.Description.Should().Be(updated.Description);
        item.Location.Should().Be(updated.Location);
        item.Quantity.Should().Be(updated.Quantity);
        item.UnitOfMeasure.Should().Be(updated.UnitOfMeasure);
        item.Vendor.Should().Be(updated.Vendor);

        item.CreatedBy.Should().Be(userId);
        item.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().NotBeEmpty();
        item.LastModified.Should().NotBeNull();

    }
}

