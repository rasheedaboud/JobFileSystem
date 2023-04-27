using Ardalis.GuardClauses;
using Core.Entities;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.Estimates.Events
{
    public class EstimateDeletedEvent : INotification
    {
        public EstimateDeletedEvent(string[] paths)
        {
            Paths = paths;
        }

        public string[] Paths { get; }
    }

    public class DeleteAllFilesAssociatedWithEstimate : INotificationHandler<EstimateDeletedEvent>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<DeleteAllFilesAssociatedWithEstimate> _logger;

        public DeleteAllFilesAssociatedWithEstimate(IAzureBlobService azureBlobService,
                                                   ILogger<DeleteAllFilesAssociatedWithEstimate> logger)
        {
            _azureBlobService = azureBlobService;
            _logger = logger;
        }

        public async Task Handle(EstimateDeletedEvent notification,
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
