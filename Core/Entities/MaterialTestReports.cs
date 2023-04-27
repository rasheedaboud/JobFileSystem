using Ardalis.GuardClauses;
using Core.Exceptions;
using Core.Features.MaterialTestReports.Events;
using JobFileSystem.Shared;
using JobFileSystem.Shared.DTOs;
using JobFileSystem.Shared.Enums;

namespace Core.Entities
{
    public class MaterialTestReport : BaseEntity
    {
        public MaterialTestReport()
        {

        }
        public MaterialTestReport(MaterialTestReportDto dto)
        {
            Guard.Against.NullOrEmpty(dto.HeatNumber, nameof(dto.HeatNumber));

            Guard.Against.NullOrEmpty(dto.MaterialType, nameof(dto.MaterialType));

            Guard.Against.NullOrEmpty(dto.MaterialGrade, nameof(dto.MaterialGrade));

            Guard.Against.NullOrEmpty(dto.MaterialForm, nameof(dto.MaterialForm));
            Guard.Against.Negative(dto.Length, nameof(dto.Length));
            Guard.Against.Negative(dto.Width, nameof(dto.Width));
            Guard.Against.Negative(dto.Diameter, nameof(dto.Diameter));
            Guard.Against.Negative(dto.Thickness, nameof(dto.Thickness));
            Guard.Against.NullOrEmpty(dto.Description, nameof(dto.Description));
            Guard.Against.Null(dto.Location, nameof(dto.Location));
            Guard.Against.Negative(dto.Quantity, nameof(dto.Quantity));
            Guard.Against.NullOrEmpty(dto.UnitOfMeasure, nameof(dto.UnitOfMeasure));
            Guard.Against.NullOrEmpty(dto.Vendor, nameof(dto.Vendor));

            HeatNumber = dto.HeatNumber;
            MaterialType = dto.MaterialType;
            MaterialGrade = dto.MaterialGrade;
            MaterialForm = MaterialForm.FromName(dto.MaterialForm);
            Length = dto.Length;
            Width = dto.Width;
            Diameter = dto.Diameter;
            Thickness = dto.Thickness;
            Description = dto.Description;
            Location = dto.Location;
            Quantity = dto.Quantity;
            UnitOfMeasure = dto.UnitOfMeasure;
            Vendor = dto.Vendor;

        }

        public MaterialTestReport Update(MaterialTestReportDto dto)
        {
            Guard.Against.NullOrEmpty(dto.HeatNumber, nameof(dto.HeatNumber));

            Guard.Against.NullOrEmpty(dto.MaterialType, nameof(dto.MaterialType));

            Guard.Against.NullOrEmpty(dto.MaterialGrade, nameof(dto.MaterialGrade));

            Guard.Against.NullOrEmpty(dto.MaterialForm, nameof(dto.MaterialForm));
            Guard.Against.Negative(dto.Length, nameof(dto.Length));
            Guard.Against.Negative(dto.Width, nameof(dto.Width));
            Guard.Against.Negative(dto.Diameter, nameof(dto.Diameter));
            Guard.Against.Negative(dto.Thickness, nameof(dto.Thickness));
            Guard.Against.NullOrEmpty(dto.Description, nameof(dto.Description));
            Guard.Against.NullOrEmpty(dto.Location, nameof(dto.Location));
            Guard.Against.Negative(dto.Quantity, nameof(dto.Quantity));
            Guard.Against.NullOrEmpty(dto.UnitOfMeasure, nameof(dto.UnitOfMeasure));
            Guard.Against.NullOrEmpty(dto.Vendor, nameof(dto.Vendor));

            HeatNumber = dto.HeatNumber;
            MaterialType = dto.MaterialType;
            MaterialGrade = dto.MaterialGrade;
            MaterialForm = MaterialForm.FromName(dto.MaterialForm);
            Length = dto.Length;
            Width = dto.Width;
            Diameter = dto.Diameter;
            Thickness = dto.Thickness;
            Description = dto.Description;
            Location = dto.Location;
            Quantity = dto.Quantity;
            UnitOfMeasure = dto.UnitOfMeasure;
            Vendor = dto.Vendor;
            return this;
        }

        public string HeatNumber { get; private set; }
        public string MaterialType { get; private set; }
        public string MaterialGrade { get; private set; }
        public MaterialForm MaterialForm { get; private set; }
        public decimal Length { get; private set; }
        public decimal Width { get; private set; }
        public decimal Diameter { get; private set; }
        public decimal Thickness { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public decimal Quantity { get; private set; }
        public string UnitOfMeasure { get; private set; }
        public string Vendor { get; private set; }

        private List<Attachment> _attachments = new List<Attachment>();
        public List<Attachment> Attachments => _attachments;


        public MaterialTestReport AddAttachment(string fileName, Stream stream)
        {
            Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
            Guard.Against.Null(stream);
            Guard.Against.InvalidInput(stream, nameof(stream), stream => stream.Length >= 0);


            var path = Path.Combine("MTRS", Id, fileName);
            var attachment = new Attachment(fileName, path);
            _attachments.Add(attachment);
            base.AddEvent(new AttachmentAddedToMaterialTestReportEvent(attachment.BlobPath, stream));
            return this;
        }

        public MaterialTestReport RemoveAttachment(string id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));
            var attachment = _attachments.Where(x => x.Id == id)
                                         .FirstOrDefault();
            if (attachment is null) throw new AttachmentNotFoundException();

            base.AddEvent(new AttachmentRemovedFromMaterialTestReportEvent(attachment.BlobPath));
            _attachments.Remove(attachment);
            return this;
        }
    }
}
