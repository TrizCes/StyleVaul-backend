using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Database.Configurations
{
    public class ModelsConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> modelBuilder)
        {
            modelBuilder.ToTable("Models");
            modelBuilder.HasKey(x => x.Id);

            modelBuilder
                .Property(x => x.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)");

            modelBuilder.Property(x => x.ResponsibleId).IsRequired().HasColumnType("INT");

            modelBuilder.Property(x => x.CollectionId).IsRequired().HasColumnType("INT");

            modelBuilder.Property(x => x.RealCost).IsRequired().HasColumnType("DECIMAL(18, 2)");

            modelBuilder
                .Property(x => x.Type)
                .IsRequired()
                .HasColumnName("ModelType")
                .HasColumnType("INT")
                .HasConversion<int>();

            modelBuilder.Property(x => x.Embroidery).IsRequired().HasColumnType("BIT");

            modelBuilder.Property(x => x.Print).IsRequired().HasColumnType("BIT");

            modelBuilder
                .HasOne(x => x.Collection)
                .WithMany(x => x.Models)
                .HasForeignKey(x => x.CollectionId);
        }
    }
}
