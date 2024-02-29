using Henry.Scheduling.Infrastructure.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace Henry.Scheduling.Infrastructure.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) :
            base(options) { }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Slot> Slots { get; set; }
    }
}