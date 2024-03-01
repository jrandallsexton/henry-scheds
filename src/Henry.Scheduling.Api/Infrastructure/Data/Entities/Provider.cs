using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System.Collections.Generic;

namespace Henry.Scheduling.Api.Infrastructure.Data.Entities
{
    public class Provider : EntityBase
    {
        public string Name { get; set; }

        public List<Slot> Slots { get; set; } = [];

        public List<Appointment> Appointments { get; set; } = [];

        public class EntityConfiguration : IEntityTypeConfiguration<Provider>
        {
            public void Configure(EntityTypeBuilder<Provider> builder)
            {
                builder.ToTable("Provider");
                builder.HasKey(t => t.Id);
            }
        }
    }
}