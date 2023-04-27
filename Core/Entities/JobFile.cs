using Ardalis.GuardClauses;
using Core.Features.JobFiles.Events;
using Core.Utils;
using JobFileSystem.Shared;
using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Enums;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.JobFiles;

namespace Core.Entities
{
    
        public class JobFile : BaseEntity, IAggregateRoot
    {
        public JobFile()
        {
        }

        public JobFile(int jobNumber, JobFileDto dto)
        {
            Guard.Against.NegativeOrZero(jobNumber, nameof(jobNumber));
            Guard.Against.NullOrEmpty(dto.Name, nameof(dto.Name));
            Guard.Against.NullOrEmpty(dto.Description, nameof(dto.Description));
            Guard.Against.NullOrEmpty(dto.ContactId, nameof(dto.ContactId));
            Guard.Against.NullOrEmpty(dto.EstimateId, nameof(dto.EstimateId));
            Guard.Against.NullOrEmpty(dto.PurchaseOrderNumber, nameof(dto.PurchaseOrderNumber));
            Guard.Against.Null(dto.DeliveryDate, nameof(dto.DeliveryDate));

            Number = $"JF-{jobNumber}";
            ShortDescription = dto.Name;
            Description = dto.Description;
            DateReceived = DateTime.UtcNow;
            Status = JobStatus.New;
            EstimateId = dto.EstimateId;
            ContactId = dto.ContactId;
            PurchaseOrderNumber = dto.PurchaseOrderNumber;
            DeliveryDate = dto.DeliveryDate.Value;
            base.AddEvent(new JobFileCreatedEvent(dto.EstimateId));
        }

        public string Number { get; private set; }
        public string ShortDescription { get; private set; }
        public string PurchaseOrderNumber { get; private set; }
        public string Description { get; private set; }
        public DateTime DateReceived { get; private set; }
        public DateTime DeliveryDate { get; private set; }
        public JobStatus Status { get; private set; }
        
        public string ContactId { get; private set; }
        public Contact Contact { get; private set; }

        public string EstimateId { get; private set; }
        public Estimate Estimate { get; private set; }

        private readonly List<Attachment> _attachments = new();
        public IReadOnlyList<Attachment> Attachments => _attachments;


        public JobFile AddAttachment(string fileName, Stream stream)
        {
            Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
            Guard.Against.Null(stream);
            Guard.Against.InvalidInput(stream, nameof(stream), stream => stream.Length >= 0);

            var path = Path.Combine("JobFiles", Number, fileName);
            var attachment = new Attachment(fileName, path);
            _attachments.Add(attachment);
            base.AddEvent(new AttachmentAddedToJobFile(attachment.BlobPath, stream));
            return this;
        }

        public JobFile RemoveAttachment(string id)
        {
            var attachment = _attachments.Where(x => x.Id == id)
                                         .FirstOrDefault();
            if (attachment is null) throw new NotFoundException(id, nameof(Attachment));

            base.AddEvent(new AttachmentRemovedFromJobFile(attachment.BlobPath));
            _attachments.Remove(attachment);
            return this;
        }
        public JobFile UpdateJobFile(JobFileDto dto)
        {

            Guard.Against.NullOrEmpty(dto.Name, nameof(dto.Name));
            Guard.Against.NullOrEmpty(dto.Description, nameof(dto.Description));
            Guard.Against.NullOrEmpty(dto.Status, nameof(dto.Status));
            Guard.Against.NullOrEmpty(dto.PurchaseOrderNumber, nameof(dto.PurchaseOrderNumber));
            Guard.Against.Null(dto.DeliveryDate, nameof(dto.DeliveryDate));


            ShortDescription = dto.Name;
            Description = dto.Description;
            Status = JobStatus.FromName(dto.Status);
            PurchaseOrderNumber = dto.PurchaseOrderNumber;
            DeliveryDate = dto.DeliveryDate.Value;

            return this;
        }

        public JobFile ValidateName(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            ShortDescription = name;
            return this;
        }
        public JobFile UpdateDescription(string description)
        {
            Guard.Against.NullOrEmpty(description, nameof(description));
            Description = description;
            return this;
        }
        public JobFile UpdateDateReceived(DateTime dateReceived)
        {
            Guard.Against.Null(dateReceived, nameof(dateReceived));
            DateReceived = dateReceived;
            return this;
        }
    }
}
