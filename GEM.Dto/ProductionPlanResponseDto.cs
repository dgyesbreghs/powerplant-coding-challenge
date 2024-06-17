using System.Text.Json.Serialization;

namespace GEM.Dto;

/// <summary>
/// The response DTO for the production plan.
/// </summary>
public class ProductionPlanResponseDto
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the payload.
    /// </summary>
    [JsonPropertyName("p")]
    public double Payload { get; set; }
}