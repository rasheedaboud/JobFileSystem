using Ardalis.GuardClauses;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.Estimates.Events
{
    public class AttachmentRemovedFromEstimateEvent : BaseDomainEvent
    {
        public AttachmentRemovedFromEstimateEvent(string path)
        {
            Guard.Against.NullOrWhiteSpace(path, nameof(path));

            Path = path;
        }
        public string Path { get; }
        public Stream Stream { get; }
    }

    public class RemoveAttachmentForEstimate : INotificationHandler<AttachmentRemovedFromEstimateEvent>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<RemoveAttachmentForEstimate> _logger;

        public RemoveAttachmentForEstimate(IAzureBlobService azureBlobService,
                                          ILogger<RemoveAttachmentForEstimate> logger)
        {
            _azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AttachmentRemovedFromEstimateEvent notification,
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
