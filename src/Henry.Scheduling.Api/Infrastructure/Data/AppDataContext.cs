﻿using Henry.Scheduling.Api.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Henry.Scheduling.Api.Infrastructure.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) :
            base(options) { }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Slot> Slots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Provider.EntityConfiguration).Assembly);
        }
    }
}