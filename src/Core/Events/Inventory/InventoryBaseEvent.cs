using DotnetEventSourcing.src.Core.Shared.Models;

namespace DotnetEventSourcing.src.Core.Events.Inventory;

public abstract class InventoryBaseEvent : Event
{
    public required string InventoryId { get; init; }
    public required int Quantity { get; init; }
}