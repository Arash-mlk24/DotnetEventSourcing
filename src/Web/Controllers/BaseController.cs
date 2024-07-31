using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotnetEventSourcing.src.Web.Controllers;

[Route("api/[controller]")]
[Controller]
public class BaseController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator _mediator = mediator;
}