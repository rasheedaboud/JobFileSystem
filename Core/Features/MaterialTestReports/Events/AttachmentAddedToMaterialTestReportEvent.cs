using Ardalis.GuardClauses;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.MaterialTestReports.Events
{
    public class AttachmentAddedToMaterialTestReportEvent : BaseDomainEvent
    {
        public AttachmentAddedToMaterialTestReportEvent(string path, Stream stream)
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

    public class UploadAttachmentForMaterialTestReport : INotificationHandler<AttachmentAddedToMaterialTestReportEvent>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<UploadAttachmentForMaterialTestReport> _logger;

        public UploadAttachmentForMaterialTestReport(IAzureBlobService azureBlobService,
                                          ILogger<UploadAttachmentForMaterialTestReport> logger)
        {
            _azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AttachmentAddedToMaterialTestReportEvent notification,
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
