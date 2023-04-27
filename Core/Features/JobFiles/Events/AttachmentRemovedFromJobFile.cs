using Ardalis.GuardClauses;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.JobFiles.Events
{
    public class AttachmentRemovedFromJobFile : BaseDomainEvent
    {
        public AttachmentRemovedFromJobFile(string path)
        {
            Guard.Against.NullOrWhiteSpace(path, nameof(path));

            Path = path;
        }
        public string Path { get; }
        public Stream Stream { get; }
    }

    public class RemoveAttachmentForJobFile : INotificationHandler<AttachmentRemovedFromJobFile>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<RemoveAttachmentForJobFile> _logger;

        public RemoveAttachmentForJobFile(IAzureBlobService azureBlobService,
                                          ILogger<RemoveAttachmentForJobFile> logger)
        {
            _azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AttachmentRemovedFromJobFile notification,
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
