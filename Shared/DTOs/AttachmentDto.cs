namespace JobFileSystem.Shared.Attachments
{
    public class AttachmentDto
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string FileName { get; init; } = "";
        public string FileExtention { get; init; } = "";
        public string ContentType { get; init; } = "";
        public string BlobPath { get; init; } = "";
        public string Link { get; set; } = "";

    }
}
