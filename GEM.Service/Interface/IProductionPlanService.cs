using GEM.Dto;

namespace GEM.Service.Interface;

/// <summary>
/// An API that describes a production plan service.
/// </summary>
public interface IProductionPlanService
{
    /// <summary>
    /// Create a response for the production plan.
    /// </summary>
    /// <param name="productionPlanRequestDto">The Dto of the production plan request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A promise to return a <see cref="List{T}"/> of <see cref="ProductionPlanResponseDto"/>.</returns>
    Task<List<ProductionPlanResponseDto>> CreateResponse(ProductionPlanRequestDto productionPlanRequestDto, CancellationToken cancellationToken = default);
}