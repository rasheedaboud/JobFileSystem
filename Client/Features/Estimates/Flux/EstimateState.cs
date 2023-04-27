using JobFileSystem.Client.Features;
using JobFileSystem.Client.Features.Attachments;
using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.Interfaces;

namespace Client.Features.Estimates
{
    public record EstimateState : BaseState<EstimateDto>, IAttachment
    {
        public string BlobUri { get; init; }

        public List<AttachmentDto> Attachments
        { 
            get => this.SelectedItem.Attachments;
            set { base.SelectedItem.Attachments = value; }
        }
 }
}
