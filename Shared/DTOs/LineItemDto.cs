using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Interfaces;
using System.Text.Json.Serialization;

namespace JobFileSystem.Shared.LineItems
{
    public class LineItemDto : IId, IAttachment
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("qty")]
        public decimal Qty { get; set; } = 0m;

        [JsonPropertyName("unitOfMeasure")]
        public string? UnitOfMeasure { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("delivery")]
        public string? Delivery { get; set; }

        [JsonPropertyName("estimatedUnitPrice")]
        public decimal EstimatedUnitPrice { get; set; } = 0m;

        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice => EstimatedUnitPrice * 1.2m;

        [JsonPropertyName("lineTotal")]
        public decimal LineTotal => UnitPrice * Qty;

        [JsonPropertyName("attachments")]
        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();

    }
}
