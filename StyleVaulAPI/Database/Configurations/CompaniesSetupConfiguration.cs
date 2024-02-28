using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Database.Configurations
{
    public class CompaniesSetupConfiguration : IEntityTypeConfiguration<CompanySetup>
    {
        public void Configure(EntityTypeBuilder<CompanySetup> builder)
        {
            builder.ToTable("CompaniesSetup");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Theme)
                .IsRequired()
                .HasColumnName("THEME")
                .HasColumnType("INT")
                .HasConversion<int>();

            builder
                .Property(x => x.Logo)
                .IsRequired()
                .HasColumnName("LOGO")
                .HasColumnType("VARCHAR(480)");

            builder.Property(x => x.CompanySetupId).IsRequired().HasColumnType("INT");

            builder
                .HasOne(x => x.Company)
                .WithMany(x => x.CompaniesSetup)
                .HasForeignKey(x => x.CompanySetupId);
        }
    }
}
