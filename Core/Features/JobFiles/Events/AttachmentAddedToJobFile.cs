using Ardalis.GuardClauses;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.JobFiles.Events
{
    public class AttachmentAddedToJobFile : BaseDomainEvent
    {
        public AttachmentAddedToJobFile(string path, Stream stream)
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

    public class UploadAttachmentForJobFile : INotificationHandler<AttachmentAddedToJobFile>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<UploadAttachmentForJobFile> _logger;

        public UploadAttachmentForJobFile(IAzureBlobService azureBlobService,
                                          ILogger<UploadAttachmentForJobFile> logger)
        {
            _azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AttachmentAddedToJobFile notification,
                                 CancellationToken cancellationToken)
        {
            try
            {
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
