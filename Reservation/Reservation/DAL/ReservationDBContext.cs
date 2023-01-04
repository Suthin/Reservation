using Microsoft.EntityFrameworkCore;
using Reservation.DAL.Models;

namespace Reservation.DAL {
    /// <summary>
    /// Its the EF DB context class for accessing database
    /// </summary>
    public class ReservationDBContext : DbContext {


        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientSlot> ClientSlots { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderSlot> ProviderSlots { get; set; }

        public string ConnectionString {
            get {

                //Read Configuration from appSettings    
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                return config.GetConnectionString("DefaultConnection") ?? string.Empty;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<ClientSlot>().ToTable("ClientSlot");
            modelBuilder.Entity<Provider>().ToTable("Provider");
            modelBuilder.Entity<ProviderSlot>().ToTable("ProviderSlot");
        }

    }
}
