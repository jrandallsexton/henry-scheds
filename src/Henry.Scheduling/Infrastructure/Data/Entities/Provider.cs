using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Henry.Scheduling.Infrastructure.Data.Entities
{
    public class Provider : EntityBase
    {
        public string Name { get; set; }

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