using DotnetEventSourcing.src.Core.Enums;
using DotnetEventSourcing.src.Core.Events.Inventory;
using DotnetEventSourcing.src.Core.Shared.Models;

namespace DotnetEventSourcing.src.Core.Entities;

public class InventoryEntity(string itemId, int quantity, InventoryEventEnum eventType) : BaseEntity
{
    public string ItemId { get; private set; } = itemId;
    public int Quantity { get; private set; } = quantity;
    public InventoryEventEnum EventType = eventType;

    public InventoryEntity() : this("", 0, InventoryEventEnum.InventoryCreated)
    {
    }

    private void Apply(InventoryCreatedEvent @event)
    {
        EventType = InventoryEventEnum.InventoryCreated;
        IsDeleted = false;
        IsVisible = true;
        CreatedAt = DateTime.Now;
        Id = Guid.NewGuid().ToString();

        ItemId = @event.InventoryId;
        Quantity = @event.Quantity;
    }

    private void Apply(InventoryQuantityChangedEvent @event)
    {
        EventType = InventoryEventEnum.InventoryQuantityChanged;

        Quantity = @event.Quantity;
    }

    public void Apply(InventoryBaseEvent @event)
    {
        switch (@event)
        {
            case InventoryCreatedEvent createdEvent:
                Apply(createdEvent);
                break;
            case InventoryQuantityChangedEvent increasedEvent:
                Apply(increasedEvent);
                break;
        }
    }
}