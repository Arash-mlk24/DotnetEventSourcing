using DotnetEventSourcing.src.Core.Commands.Inventory;
using DotnetEventSourcing.src.Core.Entities;
using DotnetEventSourcing.src.Core.Enums;
using DotnetEventSourcing.src.Core.Events.Inventory;
using DotnetEventSourcing.src.Core.Shared.Context;
using DotnetEventSourcing.src.Core.Shared.Types;
using DotnetEventSourcing.src.Infrastructure.Data.Contexts;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DotnetEventSourcing.src.Application.Commands.Inventory;

public class ChangeQuantityCommandHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IUnitOfWork<ApplicationInMemoryDbContext> inMemoryUnitOfWork
) : IRequestHandler<InventoryChangeQuantityCommand, ServiceResult<string>>
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork = unitOfWork;
    private readonly IUnitOfWork<ApplicationInMemoryDbContext> _inMemoryUnitOfWork = inMemoryUnitOfWork;

    public async Task<ServiceResult<string>> Handle(InventoryChangeQuantityCommand command, CancellationToken cancellationToken)
    {
        var serviceResult = ServiceResult.Empty;

        var inventoryProjectionRepo = _inMemoryUnitOfWork.GetRepository<InventoryEntity>();
        var inventorySourceRepo = _unitOfWork.GetRepository<InventoryEntity>();

        var inventoryStream = await inventorySourceRepo.GetQueryale()
                                                       .Where(inv => inv.ItemId == command.InventoryId)
                                                       .OrderBy(inv => inv.CreatedAt)
                                                       .ToListAsync(cancellationToken);

        if (inventoryStream.Count == 0) return serviceResult.SetError("Inventory not found.", 404).To<string>();

        var inventory = new InventoryEntity();

        inventoryStream.ForEach(item =>
        {
            switch (item.EventType)
            {
                case InventoryEventEnum.InventoryCreated:
                    inventory.Apply(item.Adapt<InventoryCreatedEvent>());
                    break;
                case InventoryEventEnum.InventoryQuantityChanged:
                    inventory.Apply(item.Adapt<InventoryQuantityChangedEvent>());
                    break;
            }
        });

        var inventoryQuantityChangedEvent = new InventoryQuantityChangedEvent
        {
            InventoryId = command.InventoryId,
            Quantity = command.Quantity
        };

        inventory.Apply(inventoryQuantityChangedEvent);

        await inventorySourceRepo.InsertAsync(inventory, cancellationToken);

        var existingInventory = await inventoryProjectionRepo.GetQueryale()
                                                             .Where(inv => inv.ItemId == command.InventoryId)
                                                             .FirstOrDefaultAsync(cancellationToken);

        if (existingInventory == null)
        {
            inventoryProjectionRepo.Insert(inventory);
        }
        else
        {
            existingInventory.Apply(inventoryQuantityChangedEvent);
            inventoryProjectionRepo.Update(existingInventory);
        }

        await _unitOfWork.SaveChangesAsync(_inMemoryUnitOfWork);

        return serviceResult.To(inventory.ItemId);
    }
}