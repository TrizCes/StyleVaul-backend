using Microsoft.EntityFrameworkCore;
using StyleVaulAPI.Models.Enums;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Database
{
    public class StyleVaulDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanySetup> CompaniesSetups { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<Model> Models { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=bd.iron.hostazul.com.br,3533;Initial Catalog=59_labclothingcollectionbd;User Id=59_user_fashion;Password=vjoprgah0eyzfikucndl;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            //TODO: alterar banco de dados quando for fazer o Migration
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var assembly = GetType().Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder
                .Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Name = "Admin",
                        Email = "admin@email.com",
                        Role = RoleEnum.Admin,
                        Password = "12345678"
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Usuário",
                        Email = "user@email.com",
                        Role = RoleEnum.ReadOnly,
                        Password = "12345678"
                    },
                    new User
                    {
                        Id = 3,
                        Name = "Gerente",
                        Email = "gerente@email.com",
                        Role = RoleEnum.Manager,
                        Password = "12345678"
                    }
                 );

            modelBuilder
                .Entity<Company>()
                .HasData(
                    new Company
                    {
                        Id = 1,
                        Cnpj = "12123123000112",
                        Email = "stylevaul@email.com",
                        Name = "Style Vaul Ltda",
                        Manager = "Beatriz Ceschini",
                        Password = "12345678"
                    });

            modelBuilder
                .Entity<Collection>()
                .HasData(
                    new Collection
                    {
                        Id = 1,
                        Name = "Estação Vibrante",
                        ResponsibleId = 1,
                        CompanyId = 1,
                        ReleaseYear = new DateTime(03, 04, 2025),
                        Season = SeasonEnum.Spring,
                        Status = StatusEnum.NotStarted,
                        Collors = "",
                        Budget = 20000,
                        Brand = "Triz"
                    });

            modelBuilder
                .Entity<Model>()
                .HasData(
                    new Model
                    {
                        Id = 1,
                        Name = "Boyfriend",
                        ResponsibleId = 1,
                        CollectionId = 1,
                        RealCost = 250.56,
                        Type = ModelTypeEnum.Bermuda,
                        Embroidery = true,
                        Print = false
                    },
                    new Model
                    {
                        Id = 2,
                        Name = "Envelope",
                        ResponsibleId = 1,
                        CollectionId = 1,
                        RealCost = 200.45,
                        Type = ModelTypeEnum.Saia,
                        Embroidery = true,
                        Print = true
                    },
                    new Model
                    {
                        Id = 3,
                        Name = "Day Party",
                        ResponsibleId = 1,
                        CollectionId = 1,
                        RealCost = 150,
                        Type = ModelTypeEnum.Camisa,
                        Embroidery = true,
                        Print = false
                    });
        }
    }
}
