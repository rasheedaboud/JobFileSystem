using Ardalis.GuardClauses;
using Core.Exceptions;
using Core.Features.LineItems.Events;
using JobFileSystem.Shared;
using JobFileSystem.Shared.LineItems;

namespace Core.Entities
{
    public class LineItem : BaseEntity
    {
        public LineItem()
        {

        }
        public LineItem(LineItemDto lineItem)
        {
            Guard.Against.NegativeOrZero(lineItem.Qty, nameof(lineItem.Qty));
            Guard.Against.NullOrEmpty(lineItem.UnitOfMeasure, nameof(lineItem.UnitOfMeasure));
            Guard.Against.NullOrEmpty(lineItem.Description, nameof(lineItem.Description));
            Guard.Against.NullOrEmpty(lineItem.Delivery, nameof(lineItem.Delivery));
            Guard.Against.Negative(lineItem.EstimatedUnitPrice, nameof(lineItem.EstimatedUnitPrice));


            EstimatedUnitPrice = lineItem.EstimatedUnitPrice;
            Delivery = lineItem.Delivery;
            UnitOfMeasure = lineItem.UnitOfMeasure;
            Description = lineItem.Description;
            Qty = lineItem.Qty;
        }
        public LineItem Update(LineItemDto lineItem)
        {
            Guard.Against.NegativeOrZero(lineItem.Qty, nameof(lineItem.Qty));
            Guard.Against.NullOrEmpty(lineItem.UnitOfMeasure, nameof(lineItem.UnitOfMeasure));
            Guard.Against.NullOrEmpty(lineItem.Description, nameof(lineItem.Description));
            Guard.Against.NullOrEmpty(lineItem.Delivery, nameof(lineItem.Delivery));
            Guard.Against.Negative(lineItem.EstimatedUnitPrice, nameof(lineItem.EstimatedUnitPrice));


            EstimatedUnitPrice = lineItem.EstimatedUnitPrice;
            Delivery = lineItem.Delivery;
            UnitOfMeasure = lineItem.UnitOfMeasure;
            Description = lineItem.Description;
            Qty = lineItem.Qty;
            return this;
        }
        public decimal Qty { get; private set; }
        public string UnitOfMeasure { get; private set; }
        public string Description { get; private set; }
        public string Delivery { get; private set; }
        public string EstimateId { get; private set; }
        public decimal EstimatedUnitPrice { get; private set; }
        public decimal UnitPrice => EstimatedUnitPrice * 1.20m;
        public decimal LineTotal => Qty * UnitPrice;

        private readonly List<Attachment> _attachments = new();
        public IReadOnlyList<Attachment> Attachments => _attachments;

        public LineItem RemoveAtachment(string id)
        {

            var attachment = _attachments.FirstOrDefault(x => x.Id == id);

            if (attachment is null) throw new AttachmentNotFoundException();

            base.AddEvent(new AttachmentRemovedFromLineItemEvent(attachment.BlobPath));
            _attachments.Remove(attachment);

            return this;
        }
        public LineItem AddAttachment(string fileName, string estimateNumber, Stream stream)
        {
            Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
            Guard.Against.Null(stream);
            Guard.Against.InvalidInput(stream, nameof(stream), stream => stream.Length >= 0);

            var path = Path.Combine("Estimates", estimateNumber, fileName);
            var attachment = new Attachment(fileName, path);
            _attachments.Add(attachment);
            base.AddEvent(new AttachmentAddedToLineItemEvent(attachment.BlobPath, stream));
            return this;
        }
    }
}
