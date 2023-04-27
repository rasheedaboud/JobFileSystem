using JobFileSystem.Client.Features;
using JobFileSystem.Shared.Contacts;

namespace Client.Features.Contacts
{
    public record ContactState : BaseState<ContactDto>
    {
        public string BlobUri { get; init; }

    }
}
