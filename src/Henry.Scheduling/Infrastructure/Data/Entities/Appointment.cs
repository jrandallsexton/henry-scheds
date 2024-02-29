using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace Henry.Scheduling.Infrastructure.Data.Entities
{
    public class Appointment : EntityBase
    {
        public DateTime? ConfirmedUtc { get; set; }

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