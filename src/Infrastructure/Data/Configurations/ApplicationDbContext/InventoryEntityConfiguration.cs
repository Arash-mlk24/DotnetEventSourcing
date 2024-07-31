using DotnetEventSourcing.src.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetEventSourcing.src.Infrastructure.Data.Configurations.ApplicationDbContext;

public class InventoryEntityConfiguration : IEntityTypeConfiguration<InventoryEntity>
{
    public InventoryEntityConfiguration()
    {
    }

    public void Configure(EntityTypeBuilder<InventoryEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType).HasConversion<int>();

        builder.ToTable($"tbl{nameof(InventoryEntity)}");
    }
}