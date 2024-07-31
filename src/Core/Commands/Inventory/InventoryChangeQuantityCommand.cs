using DotnetEventSourcing.src.Core.Shared.Types;
using MediatR;

namespace DotnetEventSourcing.src.Core.Commands.Inventory;

public record InventoryChangeQuantityCommand(string InventoryId, int Quantity) : IRequest<ServiceResult<string>>;