namespace DotnetEventSourcing.src.Web.Dtos.Input;

public class InventoryChangeQuantityInputDto(int quantity)
{
    public int Quantity { get; private set; } = quantity;
}