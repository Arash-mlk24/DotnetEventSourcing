namespace DotnetEventSourcing.src.Core.Events.Inventory;

public class InventoryQuantityChangedEvent : InventoryBaseEvent
{
    public override string StreamId => InventoryId;
}