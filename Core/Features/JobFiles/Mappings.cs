using Core.Entities;
using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Enums;
using JobFileSystem.Shared.JobFiles;

namespace Core.Features.JobFiles
{
    public static class Mappings
    {
        public static List<JobFileDto> MapJobFilesToDto(List<JobFile> entities)
        {
            return entities.Select(jobfile => MapJobFileToDto(jobfile)).ToList();
        }

        public static JobFileDto MapJobFileToDto(JobFile entity) => new()
        {
            ContactCompany = entity.Contact.Company,
            DateReceived = entity.DateReceived,
            Description = entity.Description,
            DeliveryDate = entity.DeliveryDate,
            Id = entity.Id,
            Name = entity.ShortDescription,
            Number = entity.Number,
            Attachments = entity.Attachments.Select(x => new AttachmentDto()
            {
                Id = x.Id,
                ContentType = x.ContentType,
                FileExtention = x.FileExtention,
                FileName = x.FileName,
            }).ToList(),
            Status = entity.Status.Name,
            PurchaseOrderNumber = entity.PurchaseOrderNumber,
        };

    }
}
