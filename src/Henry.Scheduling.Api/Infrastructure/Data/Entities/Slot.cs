using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Henry.Scheduling.Api.Infrastructure.Data.Entities
{
    public class Slot : EntityBase
    {

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