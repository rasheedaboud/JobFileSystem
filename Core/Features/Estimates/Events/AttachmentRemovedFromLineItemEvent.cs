using Ardalis.GuardClauses;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.LineItems.Events
{
    public class AttachmentRemovedFromLineItemEvent : BaseDomainEvent
    {
        public AttachmentRemovedFromLineItemEvent(string path)
        {
            Guard.Against.NullOrWhiteSpace(path, nameof(path));

            Path = path;
        }
        public string Path { get; }
        public Stream Stream { get; }
    }

    public class RemoveAttachmentForLineItem : INotificationHandler<AttachmentRemovedFromLineItemEvent>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<RemoveAttachmentForLineItem> _logger;

        public RemoveAttachmentForLineItem(IAzureBlobService azureBlobService,
                                          ILogger<RemoveAttachmentForLineItem> logger)
        {
            _azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AttachmentRemovedFromLineItemEvent notification,
                                 CancellationToken cancellationToken)
        {
            try
            {
                await _azureBlobService.RemoveBlob(notification.Path);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex,
                                 message: ex.Message);
                throw;
            }

        }
    }
}
