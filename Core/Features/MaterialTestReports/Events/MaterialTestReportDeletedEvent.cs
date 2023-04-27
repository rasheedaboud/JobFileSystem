using Ardalis.GuardClauses;
using Core.Entities;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.MaterialTestReports.Events
{
    public class MaterialTestReportDeletedEvent : INotification
    {
        public MaterialTestReportDeletedEvent(string[] paths)
        {
            Paths = paths;
        }

        public string[] Paths { get; }
    }

    public class DeleteAllFilesAssociatedWithMaterialTestReport : INotificationHandler<MaterialTestReportDeletedEvent>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<DeleteAllFilesAssociatedWithMaterialTestReport> _logger;

        public DeleteAllFilesAssociatedWithMaterialTestReport(IAzureBlobService azureBlobService,
                                                   ILogger<DeleteAllFilesAssociatedWithMaterialTestReport> logger)
        {
            _azureBlobService = azureBlobService;
            _logger = logger;
        }

        public async Task Handle(MaterialTestReportDeletedEvent notification,
                                 CancellationToken cancellationToken)
        {
            try
            {
                foreach (var path in notification.Paths)
                {
                    await _azureBlobService.RemoveBlob(path);
                }
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
