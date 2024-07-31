namespace DotnetEventSourcing.src.Web.Dtos.Output;

public class InventoryGetAllOutputDto(string inventoryId, int quantity)
{
    public string InventoryId { get; private set; } = inventoryId;
    public int Quantity { get; private set; } = quantity;
}