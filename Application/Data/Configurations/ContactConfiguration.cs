
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JobFileSystem.Shared.Enums;

namespace Application.Data.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.Property(p => p.ContactType)
                                   .HasConversion(
                                        p => p.Value,
                                        p => ContactType.FromValue(p));
            builder.Property(p => p.ContactMethod)
                                   .HasConversion(
                                        p => p.Value,
                                        p => ContactMethod.FromValue(p));
            builder.HasQueryFilter(x => x.IsDeleted == false);
        }
    }
}
