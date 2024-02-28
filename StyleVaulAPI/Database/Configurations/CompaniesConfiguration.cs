using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Database.Configurations
{
    public class CompaniesConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasColumnName("CompanyName")
                .HasColumnType("VARCHAR(100)");

            builder
                .Property(x => x.Cnpj)
                .IsRequired()
                .HasColumnName("Cnpj")
                .HasColumnType("VARCHAR(20)");

            builder
                .Property(x => x.Manager)
                .IsRequired()
                .HasColumnName("ManagerName")
                .HasColumnType("VARCHAR(100)");

            builder
                .Property(x => x.Email)
                .IsRequired()
                .HasColumnName("CompanyEmail")
                .HasColumnType("VARCHAR(100)");

            builder
                .Property(x => x.Password)
                .IsRequired()
                .HasColumnName("CompanyPassword")
                .HasColumnType("VARCHAR(20)");

            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
