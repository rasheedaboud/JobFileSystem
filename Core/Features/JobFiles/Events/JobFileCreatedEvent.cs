using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using JobFileSystem.Shared.Enums;
using JobFileSystem.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.JobFiles.Events
{
    public class JobFileCreatedEvent : BaseDomainEvent, INotification
    {
        public JobFileCreatedEvent(string estimateId)
        {
            EstimateId = estimateId;
        }

        public string EstimateId { get; }
    }
    public class ChangeEstimateStatusToJobFileIssued : INotificationHandler<JobFileCreatedEvent>
    {
        private readonly IRepository<Estimate> _db;
        private readonly ILogger<ChangeEstimateStatusToJobFileIssued> _logger;

        public ChangeEstimateStatusToJobFileIssued(IRepository<Estimate> db,
                                                   ILogger<ChangeEstimateStatusToJobFileIssued> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task Handle(JobFileCreatedEvent notification,
                                 CancellationToken cancellationToken)
        {
            try
            {
                var estimate = await _db.GetByIdAsync(notification.EstimateId);

                if (estimate == null) throw new NotFoundException($"No estimate with id:{notification.EstimateId} found.");

                estimate.SetStatus(EstimateStatus.JobFileIssued.Name);

                await _db.UpdateAsync(estimate);
                await _db.SaveChangesAsync();

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
