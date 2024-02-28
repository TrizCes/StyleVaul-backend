using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Database.Configurations
{
    public class CollectionsConfiguration
    {
        public class CollectionConfiguration : IEntityTypeConfiguration<Collection>
        {
            public void Configure(EntityTypeBuilder<Collection> builder)
            {
                builder.HasKey(c => c.Id);

                builder.Property(c => c.CompanyId).IsRequired();

                builder.Property(c => c.ResponsibleId).IsRequired();

                builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

                builder.Property(c => c.Brand).IsRequired().HasMaxLength(100);

                builder.Property(c => c.Budget).IsRequired().HasColumnType("decimal(18, 2)");

                builder.Property(c => c.ReleaseYear).IsRequired().HasColumnType("date");

                builder.Property(c => c.Collors).IsRequired().HasMaxLength(500);

                builder.Property(c => c.Season).IsRequired().HasConversion<int>();

                builder.Property(c => c.Status).IsRequired().HasConversion<int>();

                builder
                    .HasOne(c => c.Responsible)
                    .WithMany(u => u.Collections)
                    .HasForeignKey(r => r.ResponsibleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                builder
                    .HasOne(c => c.Company)
                    .WithMany(c => c.Collections)
                    .HasForeignKey(d => d.CompanyId)
                    .IsRequired();
            }
        }
    }
}
