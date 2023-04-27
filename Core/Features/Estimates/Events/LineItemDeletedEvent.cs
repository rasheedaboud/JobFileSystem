using JobFileSystem.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.LineItems.Events
{
    public class LineItemDeletedEvent : INotification
    {
        public LineItemDeletedEvent(string[] paths)
        {
            Paths = paths;
        }

        public string[] Paths { get; }
    }

    public class DeleteAllFilesAssociatedWithLineItem : INotificationHandler<LineItemDeletedEvent>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<DeleteAllFilesAssociatedWithLineItem> _logger;

        public DeleteAllFilesAssociatedWithLineItem(IAzureBlobService azureBlobService,
                                                   ILogger<DeleteAllFilesAssociatedWithLineItem> logger)
        {
            _azureBlobService = azureBlobService;
            _logger = logger;
        }

        public async Task Handle(LineItemDeletedEvent notification,
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
