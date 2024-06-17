using GEM.Dto;
using GEM.Service.Interface;

namespace GEM.Service;

/// <summary>
/// The implementation of the <see cref="IProductionPlanService"/> API.
/// </summary>
public class ProductionPlanService :
    IProductionPlanService
{
    /// <inheritdoc cref="IProductionPlanService.CreateResponse(ProductionPlanRequestDto, CancellationToken)" />
    protected virtual Task<List<ProductionPlanResponseDto>> CreateResponse(ProductionPlanRequestDto productionPlanRequestDto, CancellationToken cancellationToken)
    {
        var list = new List<ProductionPlanResponseDto>();
        var gasCost = 0.0;
        var kerosineCost = 0.0;
        var windTurbinePayload = 0;
        CalculateCost(productionPlanRequestDto, ref gasCost, ref kerosineCost, ref windTurbinePayload);

        double payloadProduced = 0;
        payloadProduced = AddWindTurbines(productionPlanRequestDto, list, payloadProduced);
        AddFuelPowerPlants(productionPlanRequestDto, list, gasCost, kerosineCost, payloadProduced);
        
        return Task.FromResult(list);
    }

    private static void AddFuelPowerPlants(ProductionPlanRequestDto productionPlanRequestDto, List<ProductionPlanResponseDto> list, double gasCost, double kerosineCost, double payloadProduced)
        {
            foreach (var powerplant in productionPlanRequestDto.Powerplants)
            {
                if (powerplant.Type != ProductionPlanRequestDto.PowerplantType.Windturbine && gasCost > kerosineCost && powerplant.Type == ProductionPlanRequestDto.PowerplantType.Turbojet)
                {
                    payloadProduced = CalculateProducedPayload(productionPlanRequestDto, list, payloadProduced, powerplant);
                }else if (powerplant.Type == ProductionPlanRequestDto.PowerplantType.Gasfired)
                {
                    payloadProduced = CalculateProducedPayload(productionPlanRequestDto, list, payloadProduced, powerplant);
                }
            }
        }

        private static double AddWindTurbines(ProductionPlanRequestDto productionPlanRequestDto, List<ProductionPlanResponseDto> list, double payloadProduced)
        {
            foreach (var windturbine in productionPlanRequestDto.Powerplants.Where(a => a.Type == ProductionPlanRequestDto.PowerplantType.Windturbine))
            {
                if (payloadProduced < productionPlanRequestDto.Load)
                {
                    payloadProduced = AddWindPayloadToList(productionPlanRequestDto, list, payloadProduced, windturbine);
                }
            }

            return payloadProduced;
        }

        private static void CalculateCost(ProductionPlanRequestDto productionPlanRequestDto, ref double gasCost, ref double kerosineCost, ref int windTurbinePayload)
        {
            foreach (var powerplant in productionPlanRequestDto.Powerplants)
            {
                if (powerplant != null && productionPlanRequestDto.Fuels != null)
                {
                    switch (powerplant.Type)
                    {
                        case ProductionPlanRequestDto.PowerplantType.Windturbine:
                            windTurbinePayload += Convert.ToInt32(powerplant.Pmax * powerplant.Efficiency* (productionPlanRequestDto.Fuels.WindPercentage/100));
                            break;
                        case ProductionPlanRequestDto.PowerplantType.Gasfired:
                            gasCost += (powerplant.Efficiency * powerplant.Pmax * productionPlanRequestDto.Fuels.GasEuroPerMWh) + ((powerplant.Efficiency * powerplant.Pmax * 0.3) * productionPlanRequestDto.Fuels.Co2EuroPerTon);
                            break;
                        case ProductionPlanRequestDto.PowerplantType.Turbojet:
                            kerosineCost += powerplant.Efficiency * powerplant.Pmax * productionPlanRequestDto.Fuels.KerosineEuroPerMWh;
                            break;
                    }
                }
            }
        }

        private static double CalculateProducedPayload(ProductionPlanRequestDto productionPlanRequestDto, List<ProductionPlanResponseDto> list, double payloadProduced, ProductionPlanRequestDto.Powerplant powerplant)
        {
            return payloadProduced < productionPlanRequestDto.Load 
                ? CalculateFuelPayload(productionPlanRequestDto, list, payloadProduced, powerplant) 
                : AddFuelPayload(list, payloadProduced, powerplant, 0);
        }

        private static double CalculateFuelPayload(ProductionPlanRequestDto productionPlanRequestDto, List<ProductionPlanResponseDto> list, double payloadProduced, ProductionPlanRequestDto.Powerplant powerplant)
        {
            var maxPayload = Convert.ToDouble((powerplant.Pmax * powerplant.Efficiency));
            if (productionPlanRequestDto.Load >= (maxPayload + payloadProduced))
            {
                payloadProduced = AddFuelPayload(list, payloadProduced, powerplant, maxPayload);
            }
            else
            {
                maxPayload = productionPlanRequestDto.Load - payloadProduced;
                payloadProduced = AddFuelPayload(list, payloadProduced, powerplant, maxPayload);
            }

            return payloadProduced;
        }

        private static double AddFuelPayload(List<ProductionPlanResponseDto> list, double payloadProduced, ProductionPlanRequestDto.Powerplant powerplant, double maxPayload)
        {
            list.Add(new ProductionPlanResponseDto
            {
                Name = powerplant.Name,
                Payload = maxPayload
            });
            payloadProduced += maxPayload;
            
            return payloadProduced;
        }

        private static double AddWindPayloadToList(ProductionPlanRequestDto productionPlanRequestDto, List<ProductionPlanResponseDto> list, double payloadProduced, ProductionPlanRequestDto.Powerplant powerplant)
        {
            list.Add(new ProductionPlanResponseDto
            {
                Name = powerplant.Name,
                Payload = Math.Round(Convert.ToDouble(powerplant.Pmax * powerplant.Efficiency* (productionPlanRequestDto.Fuels.WindPercentage/100)),2)
            });
            payloadProduced += Math.Round(Convert.ToDouble((powerplant.Pmax * powerplant.Efficiency)* (productionPlanRequestDto.Fuels.WindPercentage/100)));
            return payloadProduced;
        }
    
    Task<List<ProductionPlanResponseDto>> IProductionPlanService.CreateResponse(ProductionPlanRequestDto productionPlanRequestDto,
        CancellationToken cancellationToken) => CreateResponse(productionPlanRequestDto, cancellationToken);
}