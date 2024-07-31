namespace DotnetEventSourcing.src.Web.Dtos.Input;

public class InventoryCreateInputDto(int quantity)
{
    public int Quantity { get; private set; } = quantity;
}