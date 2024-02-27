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
        }
    }
}
