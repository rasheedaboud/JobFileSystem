using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JobFileSystem.Shared.DTOs
{
    public class MaterialTestReportDto: IAttachment, IId
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("heatNumber")]
        public string? HeatNumber { get; set; } = "";

        [JsonPropertyName("materialType")]
        public string? MaterialType { get;  set; } = "";

        [JsonPropertyName("materialGrade")]
        public string? MaterialGrade { get;  set; } = "";

        [JsonPropertyName("materialForm")]
        public string? MaterialForm { get;  set; } = "";

        [JsonPropertyName("length")]
        public decimal Length { get; set; } = 0;

        [JsonPropertyName("width")]
        public decimal Width { get;  set; } = 0;

        [JsonPropertyName("diameter")]
        public decimal Diameter { get;  set; } = 0;

        [JsonPropertyName("thickness")]
        public decimal Thickness { get;  set; } = 0;

        [JsonPropertyName("description")]
        public string? Description { get;  set; } = "";

        [JsonPropertyName("location")]
        public string? Location { get;  set; } = "";

        [JsonPropertyName("quantity")]
        public decimal Quantity { get;  set; } = 0;

        [JsonPropertyName("unitOfMeasure")]
        public string? UnitOfMeasure { get;  set; } = "";

        [JsonPropertyName("vendor")]
        public string? Vendor { get;  set; } = "";

        public List<AttachmentDto> Attachments { get; set; } = new();
    }
}
