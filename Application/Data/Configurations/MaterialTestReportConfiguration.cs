
using Core.Entities;
using JobFileSystem.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Configurations
{
    public class MaterialTestReportConfiguration : IEntityTypeConfiguration<MaterialTestReport>
    {
        public void Configure(EntityTypeBuilder<MaterialTestReport> builder)
        {
            builder.Property(p => p.Quantity).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Thickness).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Width).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Length).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Diameter).HasColumnType("decimal(18,4)");

            builder.Property(p => p.MaterialForm)
                                   .HasConversion(
                                        p => p.Value,
                                        p => MaterialForm.FromValue(p));
            builder.OwnsMany(
                 MaterialTestReport => MaterialTestReport.Attachments, attachments =>
                 {
                     attachments.UsePropertyAccessMode(PropertyAccessMode.Field);
                 });

            builder.HasQueryFilter(x => x.IsDeleted == false);
        }
    }
}
