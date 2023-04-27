using JobFileSystem.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.JobFiles.Events
{
    public class JobFileDeleted : INotification
    {
        public JobFileDeleted(string[] paths)
        {
            Paths = paths;
        }

        public string[] Paths { get; }
    }

    public class DeleteAllFilesAssociatedWithJobFile : INotificationHandler<JobFileDeleted>
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<DeleteAllFilesAssociatedWithJobFile> _logger;

        public DeleteAllFilesAssociatedWithJobFile(IAzureBlobService azureBlobService,
                                                   ILogger<DeleteAllFilesAssociatedWithJobFile> logger)
        {
            _azureBlobService = azureBlobService;
            _logger = logger;
        }

        public async Task Handle(JobFileDeleted notification,
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
