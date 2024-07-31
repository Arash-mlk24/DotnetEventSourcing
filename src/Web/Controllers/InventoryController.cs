using DotnetEventSourcing.src.Core.Commands.Inventory;
using DotnetEventSourcing.src.Core.Queries.Inventory;
using DotnetEventSourcing.src.Core.Shared.Context;
using DotnetEventSourcing.src.Core.Shared.Types;
using DotnetEventSourcing.src.Web.Dtos.Input;
using DotnetEventSourcing.src.Web.Dtos.Output;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotnetEventSourcing.src.Web.Controllers;

public class InventoryController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet("GetAll")]
    public async Task<ActionResult<ServiceResult<IPagedList<InventoryGetAllOutputDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var command = new GetAllInventoriesQuery();
        return await _mediator.Send(command, cancellationToken);
    }

    [HttpPost("{inventoryId}/Quantity")]
    public async Task<ActionResult<ServiceResult<string>>> ChangeQuantity([FromRoute] string inventoryId, [FromBody] InventoryChangeQuantityInputDto dto, CancellationToken cancellationToken)
    {
        var command = new InventoryChangeQuantityCommand(inventoryId, dto.Quantity);
        return await _mediator.Send(command, cancellationToken);
    }

    [HttpPost("")]
    public async Task<ActionResult<ServiceResult<string>>> Create([FromBody] InventoryCreateInputDto dto, CancellationToken cancellationToken)
    {
        var command = new InventoryCreateCommand(dto.Quantity);
        return await _mediator.Send(command, cancellationToken);
    }

    [HttpPost("Seed")]
    public async Task<ActionResult<ServiceResult<List<string>>>> Seed(CancellationToken cancellationToken)
    {
        var command = new InventorySeedCommand();
        return await _mediator.Send(command, cancellationToken);
    }
}