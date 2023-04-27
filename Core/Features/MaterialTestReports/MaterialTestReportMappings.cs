using Core.Entities;
using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.DTOs;

namespace Core.Features.MaterialTestReports
{
    public static class MaterialTestReportMapper
    {
        public static MaterialTestReportDto MapMaterialTestReportToDto(MaterialTestReport dto) => new()
        {
            Id = dto.Id,
            Attachments = dto.Attachments.Select(x => new AttachmentDto()
            {
                Id = x.Id,
                ContentType = x.ContentType,
                FileExtention = x.FileExtention,
                FileName = x.FileName,
                BlobPath = x.BlobPath,
            }).ToList(),
            HeatNumber = dto.HeatNumber,
            MaterialType = dto.MaterialType,
            MaterialGrade = dto.MaterialGrade,
            MaterialForm = dto.MaterialForm.Name,
            Length = dto.Length,
            Width = dto.Width,
            Diameter = dto.Diameter,
            Thickness = dto.Thickness,
            Description = dto.Description,
            Location = dto.Location,
            Quantity = dto.Quantity,
            UnitOfMeasure = dto.UnitOfMeasure,
            Vendor = dto.Vendor,
        };
        public static List<MaterialTestReportDto> MapMaterialTestReportsToDtos(List<MaterialTestReport> MaterialTestReports) =>
            MaterialTestReports.Select(x => MapMaterialTestReportToDto(x)).ToList();
    }
}
