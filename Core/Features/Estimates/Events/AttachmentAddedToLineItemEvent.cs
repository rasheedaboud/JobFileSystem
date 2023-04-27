using Ardalis.GuardClauses;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.LineItems.Events
{
    public class AttachmentAddedToLineItemEvent : BaseDomainEvent
    {
        public AttachmentAddedToLineItemEvent(string path, Stream stream)
        {
            Guard.Against.NullOrWhiteSpace(path, nameof(path));
            Guard.Against.Null(stream);
            Guard.Against.InvalidInput(stream, nameof(stream), stream => stream.Length >= 0);

            Path = path;
            Stream = stream;
        }
        public string Path { get; }
        public Stream Stream { get; }
    }

    public class UploadAttachmentForLineItem : INotificationHandler<AttachmentAddedToLineItemEvent>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<UploadAttachmentForLineItem> _logger;

        public UploadAttachmentForLineItem(IAzureBlobService azureBlobService,
                                          ILogger<UploadAttachmentForLineItem> logger)
        {
            _azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AttachmentAddedToLineItemEvent notification,
                                 CancellationToken cancellationToken)
        {
            try
            {
                notification.Stream.Position = 0;
                await _azureBlobService.UploadStreamToAzure(notification.Path,
                                                            notification.Stream);
                await notification.Stream.DisposeAsync();
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
