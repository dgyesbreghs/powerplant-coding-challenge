using GEM.Application.Requests;
using GEM.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GEM.Api.Controllers;

/// <summary>
/// The production plan controller.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ProductionPlanController : ControllerBase
{
    private readonly ILogger<ProductionPlanController> _Logger;
    private readonly IMediator _Mediator;

    public ProductionPlanController(
        ILogger<ProductionPlanController> logger,
        IMediator mediator)
    {
        _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    /// <summary>
    /// Calculate the production plan
    /// </summary>
    /// <returns>A collection of <see cref="ProductionPlanResponseDto"/> API's.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(List<ProductionPlanResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CalculateProductionPlan([FromBody] ProductionPlanRequestDto productionPlanRequestDto)
    {
        _Logger.LogDebug("Calculating the production plan");
        var response = await _Mediator.Send(new CalculateProductionPlanRequest { ProductionPlanRequestDto = productionPlanRequestDto });
        
        return Ok(response);
    }
}