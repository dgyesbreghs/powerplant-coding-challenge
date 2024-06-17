using GEM.Application.Requests;
using GEM.Dto;
using GEM.Service.Interface;
using MediatR;

namespace GEM.Application.Handlers;

/// <summary>
/// The implementation of the <see cref="IRequestHandler{TRequest,TResponse}"/> API.
/// </summary>
public class CalculateProductionPlanHandler : IRequestHandler<CalculateProductionPlanRequest, List<ProductionPlanResponseDto>>
{
    private readonly IProductionPlanService _ProductionPlanService;

    /// <summary>
    /// Create an instance.
    /// </summary>
    /// <param name="productionPlanService">The implementation of the <see cref="IProductionPlanService"/> API.</param>
    public CalculateProductionPlanHandler(IProductionPlanService productionPlanService)
    {
        _ProductionPlanService = productionPlanService ?? throw new ArgumentNullException(nameof(productionPlanService));
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle(TRequest, CancellationToken)" />
    protected virtual Task<List<ProductionPlanResponseDto>> Handle(CalculateProductionPlanRequest request, CancellationToken cancellationToken)
    {
        return _ProductionPlanService.CreateResponse(request.ProductionPlanRequestDto, cancellationToken); 
    }

    Task<List<ProductionPlanResponseDto>> IRequestHandler<CalculateProductionPlanRequest, List<ProductionPlanResponseDto>>.Handle(
        CalculateProductionPlanRequest request, CancellationToken cancellationToken) => Handle(request, cancellationToken);
}