namespace DotnetEventSourcing.src.Core.Events.Inventory;

public class InventoryCreatedEvent : InventoryBaseEvent
{
    public override string StreamId => InventoryId;
}