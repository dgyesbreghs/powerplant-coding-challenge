using GEM.Dto;
using MediatR;

namespace GEM.Application.Requests;

/// <summary>
/// The request to calculate the production plan.
/// </summary>
public class CalculateProductionPlanRequest : IRequest<List<ProductionPlanResponseDto>>
{
    /// <summary>
    /// Gets or sets the production plan request DTO.
    /// </summary>
    public ProductionPlanRequestDto ProductionPlanRequestDto { get; set; }
}