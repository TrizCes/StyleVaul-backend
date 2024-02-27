using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StyleVaul.Models;

namespace StyleVaul.Database.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100).IsUnicode(false);

            builder
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasAnnotation(
                    "RegularExpression",
                    @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b"
                );

            builder.Property(u => u.Role).IsRequired().HasConversion<int>();

            builder.Property(e => e.Password).HasMaxLength(20).IsRequired();

            builder
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId);
        }
    }
}