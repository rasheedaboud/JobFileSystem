using Core.Entities;
using Core.Specifications.JobFiles;
using JobFileSystem.Shared;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.JobFiles;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.Estimates.Events
{
    public class EstimateAcceptedEvent :BaseDomainEvent, INotification
    {
        public EstimateAcceptedEvent(Estimate estimate, string pruchaseOrderNumber)
        {
            Estimate = estimate;
            PruchaseOrderNumber = pruchaseOrderNumber;
        }

        public Estimate Estimate { get; }
        public string PruchaseOrderNumber { get; }
    }

    public class CreateNewJobFile : INotificationHandler<EstimateAcceptedEvent>
    {
        private readonly IRepository<JobFile> _db;
        private readonly ILogger<CreateNewJobFile> _logger;

        public CreateNewJobFile(IRepository<JobFile> db,
                                                   ILogger<CreateNewJobFile> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task Handle(EstimateAcceptedEvent notification,
                                 CancellationToken cancellationToken)
        {
            try
            {
                var jobFileExists = await _db.GetBySpecAsync(new GetJobFileByPurchaseOrderNumber(notification.PruchaseOrderNumber));
                
                //PREVENT DUPLICATES BASED ON PO NUMBER
                if(jobFileExists == null)
                {
                    var count = (await _db.CountAsync()) + 1;

                    var jobFile = new JobFile(count, new JobFileDto()
                    {
                        Name = notification.Estimate.ShortDescription,
                        Description = notification.Estimate.LongDescription,
                        ContactCompany = notification.Estimate.Client.Company,
                        EstimateId = notification.Estimate.Id,
                        ContactId = notification.Estimate.Client.Id,
                        PurchaseOrderNumber = notification.PruchaseOrderNumber,
                        DeliveryDate = notification.Estimate.DeliveryDate,
                    });



                    await _db.AddAsync(jobFile);
                    await _db.SaveChangesAsync();
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
