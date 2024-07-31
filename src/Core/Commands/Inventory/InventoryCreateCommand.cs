using DotnetEventSourcing.src.Core.Shared.Types;
using MediatR;

namespace DotnetEventSourcing.src.Core.Commands.Inventory;

public record InventoryCreateCommand(int Quantity) : IRequest<ServiceResult<string>>;