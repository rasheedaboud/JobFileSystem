
using JobFileSystem.Shared.Attachments;

namespace JobFileSystem.Shared.Interfaces
{
    public interface IAttachment
    {
        public List<AttachmentDto> Attachments { get; set; }

    }
}
