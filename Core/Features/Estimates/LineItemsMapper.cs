using Core.Entities;
using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.LineItems;

namespace Core.Features.Estimates
{
    public static class LineItemsMapper
    {
        public static LineItemDto MapLineItemsToDto(LineItem lineItem) => new LineItemDto()
        {
            Id = lineItem.Id,
            Qty = lineItem.Qty,
            UnitOfMeasure = lineItem.UnitOfMeasure,
            Description = lineItem.Description,
            Delivery = lineItem.Delivery,
            EstimatedUnitPrice = lineItem.EstimatedUnitPrice,

            Attachments = lineItem.Attachments.Select(x => new AttachmentDto()
            {
                Id = x.Id,
                ContentType = x.ContentType,
                FileExtention = x.FileExtention,
                FileName = x.FileName,
                BlobPath = x.BlobPath,
            }).ToList()
        };
        public static List<LineItemDto> MapLineItemsToDto(IReadOnlyList<LineItem> lineItems) => 
            lineItems.Select(x => MapLineItemsToDto(x)).ToList();
    }
}
