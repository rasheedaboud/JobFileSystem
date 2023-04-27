using JobFileSystem.Client.Features;
using JobFileSystem.Shared.JobFiles;

namespace Client.Features.JobFiles
{
    public record JobFileState : BaseState<JobFileDto>
    {
        public string BlobUri { get; init; }

    }
}
