using JobFileSystem.Client.Features;
using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.DTOs;
using JobFileSystem.Shared.Interfaces;

namespace Client.Features.MaterialTestReports
{
    public record MaterialTestReportState : BaseState<MaterialTestReportDto>, IAttachment
    {
        public List<AttachmentDto> Attachments
        {
            get => this.SelectedItem.Attachments;
            set { base.SelectedItem.Attachments = value; }
        }
    }
}
