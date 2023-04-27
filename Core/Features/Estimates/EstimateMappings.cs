using Core.Entities;
using Core.Features.Contacts;
using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Estimates;

namespace Core.Features.Estimates
{
    public static class EstimateMapper
    {
        public static EstimateDto MapEstimateToDto(Estimate estimate) => new()
        {
            Id = estimate.Id,
            Attachments = estimate.Attachments.Select(x => new AttachmentDto()
            {
                Id = x.Id,
                ContentType = x.ContentType,
                FileExtention = x.FileExtention,
                FileName = x.FileName,
                BlobPath = x.BlobPath,
            }).ToList(),
            Client = estimate.Client == null ? null : ContactMapper.MapContactToDto(estimate.Client),
            LineItems = estimate.LineItems == null ? new() : LineItemsMapper.MapLineItemsToDto(estimate.LineItems),
            LoggedOn = estimate.RequestForQuoteReceived.ToString("yyyy-MM-dd"),
            LongDescription = estimate.LongDescription,
            Number = estimate.Number,
            ShortDescription = estimate.ShortDescription,
            Status = estimate.Status.Name,
        };
        public static List<EstimateDto> MapEstimatesToDtos(List<Estimate> estimates) =>
            estimates.Select(x => MapEstimateToDto(x)).ToList();
    }
}
