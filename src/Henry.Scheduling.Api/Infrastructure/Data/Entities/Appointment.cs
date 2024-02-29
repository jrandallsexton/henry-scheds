using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Henry.Scheduling.Api.Infrastructure.Data.Entities
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