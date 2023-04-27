using JobFileSystem.Client.Features;
using JobFileSystem.Shared.LineItems;

namespace Client.Features.Estimates
{
    public record LineItemState : BaseState<LineItemDto>
    {
        public string BlobUri { get; init; }

    }
}
