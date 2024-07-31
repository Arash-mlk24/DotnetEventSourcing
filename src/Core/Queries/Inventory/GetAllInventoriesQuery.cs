using DotnetEventSourcing.src.Core.Shared.Context;
using DotnetEventSourcing.src.Core.Shared.Types;
using DotnetEventSourcing.src.Web.Dtos.Output;
using MediatR;

namespace DotnetEventSourcing.src.Core.Queries.Inventory;

public record GetAllInventoriesQuery() : IRequest<ServiceResult<IPagedList<InventoryGetAllOutputDto>>>;