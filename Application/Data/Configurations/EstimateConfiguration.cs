
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JobFileSystem.Shared.Enums;

namespace Application.Data.Configurations
{
    public class EstimateConfiguration : IEntityTypeConfiguration<Estimate>
    {
        public void Configure(EntityTypeBuilder<Estimate> builder)
        {
            builder.HasOne<JobFile>().WithOne(x=>x.Estimate).OnDelete(DeleteBehavior.ClientSetNull);


            builder.Property(p => p.Status)
                                   .HasConversion(
                                        p => p.Value,
                                        p => EstimateStatus.FromValue(p));
            builder.OwnsMany(
                 estimate => estimate.Attachments, attachments =>
                 {
                     attachments.UsePropertyAccessMode(PropertyAccessMode.Field);
                 });
            builder.OwnsMany(
                estimate => estimate.LineItems, lineItems =>
                {
                    lineItems.Property(p => p.Qty).HasColumnType("decimal(18,4)"); 
                    lineItems.Property(p => p.EstimatedUnitPrice).HasColumnType("decimal(18,4)");
                    lineItems.OwnsMany(lineItem => lineItem.Attachments, attachments =>
                    {                        
                        attachments.UsePropertyAccessMode(PropertyAccessMode.Field);
                    });
                    lineItems.UsePropertyAccessMode(PropertyAccessMode.Field);
                });

            builder.HasQueryFilter(x => x.IsDeleted == false);
        }
    }
}
