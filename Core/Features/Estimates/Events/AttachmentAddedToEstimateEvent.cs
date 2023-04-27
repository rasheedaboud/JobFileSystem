using Ardalis.GuardClauses;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.Estimates.Events
{
    public class AttachmentAddedToEstimateEvent : BaseDomainEvent
    {
        public AttachmentAddedToEstimateEvent(string path, Stream stream)
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

    public class UploadAttachmentForEstimate : INotificationHandler<AttachmentAddedToEstimateEvent>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<UploadAttachmentForEstimate> _logger;

        public UploadAttachmentForEstimate(IAzureBlobService azureBlobService,
                                          ILogger<UploadAttachmentForEstimate> logger)
        {
            _azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AttachmentAddedToEstimateEvent notification,
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
