using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Enums;
using JobFileSystem.Shared.Interfaces;
using System.Text.Json.Serialization;

namespace JobFileSystem.Shared.JobFiles
{

    public class JobFileDto :IId,IAttachment
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("number")]
        public string? Number { get;  set; }

        [JsonPropertyName("name")]
        public string? Name { get;  set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("estimateId")]
        public string? EstimateId { get; set; }

        [JsonPropertyName("purchaseOrderNumber")]
        public string? PurchaseOrderNumber { get; set; }

        [JsonPropertyName("contactId")]
        public string? ContactId { get; set; }

        [JsonPropertyName("contactCompany")]
        public string? ContactCompany { get; set; }

        [JsonPropertyName("dateReceived")]
        public DateTime? DateReceived { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("deliveryDate")]
        public DateTime? DeliveryDate { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("attachments")]
        public List<AttachmentDto>? Attachments { get; set; } = new();

        [JsonPropertyName("status")]
        public string? Status { get; set; } = JobStatus.New.Name;
    }
}
