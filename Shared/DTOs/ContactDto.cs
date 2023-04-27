using JobFileSystem.Shared.Interfaces;
using System.Text.Json.Serialization;

namespace JobFileSystem.Shared.Contacts
{
    public class ContactDto:IId
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = "";

        [JsonPropertyName("name")]
        public string? Name { get; set; } = "";

        [JsonPropertyName("company")]
        public string? Company { get; set; } = "";

        [JsonPropertyName("email")]
        public string? Email { get; set; } = "";

        [JsonPropertyName("phone")]
        public string? Phone { get; set; } = "";

        [JsonPropertyName("contactMethod")]
        public string? ContactMethod { get; set; } = "";

        [JsonPropertyName("contactType")]
        public string? ContactType { get; set; } = "";
    }
}
