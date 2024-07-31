using DotnetEventSourcing.src.Core.Entities;
using DotnetEventSourcing.src.Core.Queries.Inventory;
using DotnetEventSourcing.src.Core.Shared.Context;
using DotnetEventSourcing.src.Core.Shared.Types;
using DotnetEventSourcing.src.Infrastructure.Data.Contexts;
using DotnetEventSourcing.src.Web.Dtos.Output;
using MediatR;

namespace DotnetEventSourcing.src.Application.Queries.Inventory;

public class GetAllInventoriesQueryHandler(
    IUnitOfWork<ApplicationInMemoryDbContext> inMemoryUnitOfWork
) : IRequestHandler<GetAllInventoriesQuery, ServiceResult<IPagedList<InventoryGetAllOutputDto>>>
{
    public IUnitOfWork<ApplicationInMemoryDbContext> _inMemoryUnitOfWork = inMemoryUnitOfWork;

    public Task<ServiceResult<IPagedList<InventoryGetAllOutputDto>>> Handle(GetAllInventoriesQuery request, CancellationToken cancellationToken)
    {
        var inMemoryInventoryRepo = _inMemoryUnitOfWork.GetRepository<InventoryEntity>();

        var pagedInventories = inMemoryInventoryRepo.GetPagedList(
            predicate: inv => inv.IsDeleted == false && inv.IsVisible == true,
            pageIndex: 0,
            pageSize: 10,
            orderBy: queryable => queryable.OrderByDescending(inv => inv.CreatedAt),
            selector: inv => new InventoryGetAllOutputDto(inv.ItemId, inv.Quantity)
        );

        return Task.FromResult(ServiceResult.Empty.To(pagedInventories));
    }
}