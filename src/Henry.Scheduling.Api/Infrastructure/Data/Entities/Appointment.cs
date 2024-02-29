using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace Henry.Scheduling.Api.Infrastructure.Data.Entities
{
    public class Appointment : EntityBase
    {
        public Guid SlotId { get; set; }

        public Guid ClientId { get; set; }

        public DateTime? ConfirmedUtc { get; set; }

        public DateTime? ExpiredUtc { get; set; }

        public class EntityConfiguration : IEntityTypeConfiguration<Appointment>
        {
            public void Configure(EntityTypeBuilder<Appointment> builder)
            {
                builder.ToTable("Appointment");
                builder.HasKey(t => t.Id);
            }
        }
    }
}