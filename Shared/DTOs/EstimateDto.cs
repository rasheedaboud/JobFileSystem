using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.Enums;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.LineItems;
using System.Text.Json.Serialization;

namespace JobFileSystem.Shared.Estimates
{
    public class EstimateDto : IId , IAttachment
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("number")]
        public string? Number { get; set; }

        [JsonPropertyName("shortDescription")]
        public string? ShortDescription { get; set; }

        [JsonPropertyName("longDescription")]
        public string? LongDescription { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; } = EstimateStatus.New.Name;

        [JsonPropertyName("client")]
        public ContactDto? Client { get; set; }

        [JsonPropertyName("loggedOn")]
        public string? LoggedOn { get; set; }

        [JsonPropertyName("deliveryDate")]
        public DateTime? DeliveryDate { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("subtotal")]
        public decimal Subtotal => LineItems.Sum(x => x.LineTotal);

        [JsonPropertyName("gst")]
        public decimal Gst => Subtotal * 0.05m;

        [JsonPropertyName("total")]
        public decimal Total => Subtotal + Gst;

        [JsonPropertyName("canEditd")]
        public bool CanEdit => (Status == EstimateStatus.New.Name ||
                                Status == EstimateStatus.InitialReview.Name);

        [JsonPropertyName("jobFileIssued")]
        public bool JobFileIssued => Status == EstimateStatus.JobFileIssued.Name;

        [JsonPropertyName("attachments")]
        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();

        [JsonPropertyName("lineItems")]
        public List<LineItemDto> LineItems { get; set; } = new List<LineItemDto>();
    }
}
