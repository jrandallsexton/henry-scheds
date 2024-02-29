using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace Henry.Scheduling.Api.Infrastructure.Data.Entities
{
    public class Slot : EntityBase
    {
        public Guid ProviderId { get; set; }

        public Guid? AppointmentId { get; set; }

        public DateTime StartUtc { get; set; }

        public DateTime EndUtc { get; set; }

        public class EntityConfiguration : IEntityTypeConfiguration<Slot>
        {
            public void Configure(EntityTypeBuilder<Slot> builder)
            {
                builder.ToTable("Slot");
                builder.HasKey(t => t.Id);
            }
        }
    }
}