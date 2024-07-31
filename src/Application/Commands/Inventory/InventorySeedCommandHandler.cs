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

public class InventorySeedCommandHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IUnitOfWork<ApplicationInMemoryDbContext> inMemoryUnitOfWork
) : IRequestHandler<InventorySeedCommand, ServiceResult<List<string>>>
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork = unitOfWork;
    private readonly IUnitOfWork<ApplicationInMemoryDbContext> _inMemoryUnitOfWork = inMemoryUnitOfWork;

    public async Task<ServiceResult<List<string>>> Handle(InventorySeedCommand command, CancellationToken cancellationToken)
    {
        var serviceResult = ServiceResult.Empty;
        var r = new Random();

        var inventoryProjectionRepo = _inMemoryUnitOfWork.GetRepository<InventoryEntity>();
        var inventorySourceRepo = _unitOfWork.GetRepository<InventoryEntity>();

        var baseInventories = inventorySourceRepo.GetAll(x => true).Items.ToList();
        // for (int i = 0; i < 3; i++)
        // {
        //     var inventory = new InventoryEntity();

        //     var inventoryCreatedEvent = new InventoryCreatedEvent
        //     {
        //         InventoryId = Guid.NewGuid().ToString(),
        //         CreatedAt = DateTime.UtcNow,
        //         Quantity = r.Next(1, 10)
        //     };

        //     inventory.Apply(inventoryCreatedEvent);

        //     baseInventories.Add(inventory);
        // }

        // await inventorySourceRepo.InsertAsync(baseInventories, cancellationToken);
        // await inventoryProjectionRepo.InsertAsync(baseInventories, cancellationToken);
        // await _unitOfWork.SaveChangesAsync(_inMemoryUnitOfWork);


        var updateCommands = new List<InventoryChangeQuantityCommand>();

        baseInventories.ForEach(inv =>
        {
            for (int i = 0; i < 299; i++)
            {
                updateCommands.Add(new InventoryChangeQuantityCommand(inv.ItemId, r.Next(1, 10)));
            }
        });

        foreach (var updateCommand in updateCommands)
        {
            var inventoryStream = await inventorySourceRepo.GetQueryale()
                                                           .Where(inv => inv.ItemId == updateCommand.InventoryId)
                                                           .OrderBy(inv => inv.CreatedAt)
                                                           .ToListAsync(cancellationToken);

            if (inventoryStream.Count == 0) return serviceResult.SetError("Inventory not found.", 404).To<List<string>>();

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
                InventoryId = updateCommand.InventoryId,
                Quantity = updateCommand.Quantity
            };

            inventory.Apply(inventoryQuantityChangedEvent);

            await inventorySourceRepo.InsertAsync(inventory, cancellationToken);

            var existingInventory = await inventoryProjectionRepo.GetQueryale()
                                                                 .Where(inv => inv.ItemId == updateCommand.InventoryId)
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
        };

        return serviceResult.To(baseInventories.Select(inv => inv.ItemId).ToList());
    }
}