using Ardalis.GuardClauses;
using Core.Exceptions;
using Core.Features.Estimates.Events;
using Core.Utils;
using JobFileSystem.Shared;
using JobFileSystem.Shared.Enums;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.LineItems;

namespace Core.Entities
{

    public class Estimate : BaseEntity, IAggregateRoot
    {
        public Estimate()
        {
        }

        public Estimate(EstimateDto dto)
        {

            Guard.Against.NullOrEmpty(dto.ShortDescription, nameof(dto.ShortDescription));
            Guard.Against.NullOrEmpty(dto.LongDescription, nameof(dto.LongDescription));
            Guard.Against.NullOrEmpty(dto.Client.Id, nameof(dto.Client.Id));
            Guard.Against.Null(dto.DeliveryDate);

            Number = NumberGenerator.EstimateNumber;
            ShortDescription = dto.ShortDescription;
            LongDescription = dto.LongDescription;
            Status = EstimateStatus.New;
            ClientId = dto.Client.Id;
            DeliveryDate = dto.DeliveryDate.Value;
            RequestForQuoteReceived = DateTime.UtcNow;
        }

        public string Number { get; private set; }
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set; }
        public EstimateStatus Status { get; private set; }

        public string ClientId { get; private set; }
        public Contact Client { get; private set; }
        public DateTime RequestForQuoteReceived { get; private set; }
        public DateTime DeliveryDate { get; private set; }

        public decimal Subtotal => _lineItems.Sum(x => x.LineTotal);
        public decimal Gst => Subtotal * 0.05m;
        public decimal Total => Subtotal + Gst;

        private readonly List<Attachment> _attachments = new();
        public IReadOnlyList<Attachment> Attachments => _attachments;

        private readonly List<LineItem> _lineItems = new();
        public IReadOnlyList<LineItem> LineItems => _lineItems;

        public (Estimate,LineItemDto) AddLineItem(LineItemDto dto)
        {
            var item = new LineItem(dto);
            dto.Id = item.Id;
            _lineItems.Add(item);

            return (this, dto);
        }
        public (Estimate, LineItemDto) UpdateLineItem(LineItemDto newLineItem)
        {
            var oldLineItem = _lineItems.FirstOrDefault(x => x.Id == newLineItem.Id);
            if (oldLineItem is null)
                throw new LineItemNotFoundException();

            var updatedLineItem = oldLineItem.Update(newLineItem);


            _lineItems.Remove(oldLineItem);
            _lineItems.Add(updatedLineItem);

            return (this, newLineItem);
        }


        public Estimate RemoveLineItem(string itemId)
        {
            var item = _lineItems.FirstOrDefault(x => x.Id == itemId);
            if(item is null)
                throw new LineItemNotFoundException();

            _lineItems.Remove(item);
            return this;
        }
        public Estimate RemoveLineItemAttachment(string id, string attachmentId)
        {
            var lineItem = _lineItems.FirstOrDefault(x => x.Id == id);
            if (lineItem is null)
                throw new LineItemNotFoundException();

            lineItem.RemoveAtachment(attachmentId);

            return this;
        }

        public Estimate AddLineItemAttachment(string lineId,string fileName, Stream stream)
        {
            Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
            Guard.Against.Null(stream);
            Guard.Against.InvalidInput(stream, nameof(stream), stream => stream.Length >= 0);

            //GET RID OF HYPHEN
            var number = Number.Split("-").Last();
            var item = LineItems.FirstOrDefault(x=>x.Id == lineId);
            item.AddAttachment(fileName, number, stream);
            
            return this;
        }

        public Estimate AddAttachment(string fileName, Stream stream)
        {
            Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
            Guard.Against.Null(stream);
            Guard.Against.InvalidInput(stream, nameof(stream), stream => stream.Length >= 0);

            //GET RID OF HYPHEN
            var numer = Number.Split("-").Last();

            var path = Path.Combine("Estimates", numer, fileName);
            var attachment = new Attachment(fileName, path);
            _attachments.Add(attachment);
            base.AddEvent(new AttachmentAddedToEstimateEvent(attachment.BlobPath, stream));
            return this;
        }

        public Estimate RemoveAttachment(string id)
        {
            var attachment = _attachments.Where(x => x.Id == id)
                                         .FirstOrDefault();
            if (attachment is null) throw new AttachmentNotFoundException();

            base.AddEvent(new AttachmentRemovedFromEstimateEvent(attachment.BlobPath));
            _attachments.Remove(attachment);
            return this;
        }
        public Estimate UpdateEstimate(string id,
                                       string shortDescription,
                                       string longDescription,
                                       string status,
                                       string clientId,
                                       DateTime? deliveryDate,
                                       string purchaseOrderNumber=null)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));
            Guard.Against.NullOrEmpty(shortDescription, nameof(shortDescription));
            Guard.Against.NullOrEmpty(longDescription, nameof(longDescription));
            Guard.Against.NullOrEmpty(status, nameof(status));
            Guard.Against.NullOrEmpty(clientId, nameof(clientId));
            Guard.Against.Null(deliveryDate);

            Id = id;
            SetStatus(status, purchaseOrderNumber);
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            DeliveryDate = deliveryDate.Value;
            ClientId = clientId;
            return this;
        }

        public void SetStatus(string status,string purchaseOrderNumber=null)
        {
            Status = EstimateStatus.FromName(status);
            
            if(Status == EstimateStatus.Accepted && !string.IsNullOrEmpty(purchaseOrderNumber))
            {
                AddEvent(new EstimateAcceptedEvent(this, purchaseOrderNumber));
            }

        }


    }
}
