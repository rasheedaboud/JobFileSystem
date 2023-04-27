
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JobFileSystem.Shared.Enums;

namespace Application.Data.Configurations
{
    public class JobFileConfiguration : IEntityTypeConfiguration<JobFile>
    {
        public void Configure(EntityTypeBuilder<JobFile> builder)
        {

            builder.OwnsMany(
                jobFile => jobFile.Attachments, attachments =>
                {
                    attachments.UsePropertyAccessMode(PropertyAccessMode.Field);
                });

            builder.Property(p => p.Status)
                   .HasConversion(
                        p => p.Value,
                        p => JobStatus.FromValue(p));

            builder.HasQueryFilter(x => x.IsDeleted == false);
        }
    }
}
