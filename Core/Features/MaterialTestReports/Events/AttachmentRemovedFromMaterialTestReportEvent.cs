using Ardalis.GuardClauses;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.MaterialTestReports.Events
{
    public class AttachmentRemovedFromMaterialTestReportEvent : BaseDomainEvent
    {
        public AttachmentRemovedFromMaterialTestReportEvent(string path)
        {
            Guard.Against.NullOrWhiteSpace(path, nameof(path));

            Path = path;
        }
        public string Path { get; }
        public Stream Stream { get; }
    }

    public class RemoveAttachmentForMaterialTestReport : INotificationHandler<AttachmentRemovedFromMaterialTestReportEvent>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<RemoveAttachmentForMaterialTestReport> _logger;

        public RemoveAttachmentForMaterialTestReport(IAzureBlobService azureBlobService,
                                          ILogger<RemoveAttachmentForMaterialTestReport> logger)
        {
            _azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AttachmentRemovedFromMaterialTestReportEvent notification,
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
