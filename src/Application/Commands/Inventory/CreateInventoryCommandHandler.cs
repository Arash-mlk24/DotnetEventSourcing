using DotnetEventSourcing.src.Core.Commands.Inventory;
using DotnetEventSourcing.src.Core.Entities;
using DotnetEventSourcing.src.Core.Events.Inventory;
using DotnetEventSourcing.src.Core.Shared.Context;
using DotnetEventSourcing.src.Core.Shared.Types;
using DotnetEventSourcing.src.Infrastructure.Data.Contexts;
using Mapster;
using MediatR;

namespace DotnetEventSourcing.src.Application.Commands.Inventory;

public class CreateInventoryCommandHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IUnitOfWork<ApplicationInMemoryDbContext> inMemoryUnitOfWork
) : IRequestHandler<InventoryCreateCommand, ServiceResult<string>>
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork = unitOfWork;
    private readonly IUnitOfWork<ApplicationInMemoryDbContext> _inMemoryUnitOfWork = inMemoryUnitOfWork;

    public async Task<ServiceResult<string>> Handle(InventoryCreateCommand command, CancellationToken cancellationToken)
    {
        var serviceResult = ServiceResult.Empty;

        var inventoryProjectionRepo = _inMemoryUnitOfWork.GetRepository<InventoryEntity>();
        var inventorySourceRepo = _unitOfWork.GetRepository<InventoryEntity>();

        var inventory = new InventoryEntity();

        var inventoryCreatedEvent = new InventoryCreatedEvent
        {
            InventoryId = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            Quantity = command.Quantity
        };

        inventory.Apply(inventoryCreatedEvent);

        await inventorySourceRepo.InsertAsync(inventory, cancellationToken);
        await inventoryProjectionRepo.InsertAsync(inventory, cancellationToken);

        await _unitOfWork.SaveChangesAsync(_inMemoryUnitOfWork);

        return serviceResult.To(inventory.ItemId);
    }
}