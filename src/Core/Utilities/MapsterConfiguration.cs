using DotnetEventSourcing.src.Core.Commands.Inventory;
using DotnetEventSourcing.src.Core.Entities;
using DotnetEventSourcing.src.Core.Events.Inventory;
using Mapster;

namespace DotnetEventSourcing.src.Core.Utilities;

public class MapsterConfiguration
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<InventoryEntity, InventoryCreatedEvent>()
              .Map(d => d.InventoryId, s => s.ItemId);

        config.NewConfig<InventoryEntity, InventoryQuantityChangedEvent>()
              .Map(d => d.InventoryId, s => s.ItemId);
    }
}