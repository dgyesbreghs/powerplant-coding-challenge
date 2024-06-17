using System.Text.Json.Serialization;

namespace GEM.Dto;

/// <summary>
/// The request DTO for the production plan.
/// </summary>
public class ProductionPlanRequestDto
{
    /// <summary>
    /// Gets or sets the load.
    /// </summary>
    public int Load { get; set; }
    
    /// <summary>
    /// Gets or sets the fuels.
    /// </summary>
    public Fuel Fuels { get; set; }
    
    /// <summary>
    /// Gets or sets the powerplants.
    /// </summary>
    public IList<Powerplant> Powerplants { get; set; }

    /// <summary>
    /// A representation of the fuel.
    /// </summary>
    public class Fuel
    {
        /// <summary>
        /// Gets or sets the gas euro per MWh.
        /// </summary>
        public double GasEuroPerMWh { get; set; }
        
        /// <summary>
        /// Gets or sets the kerosine euro per MWh.
        /// </summary>
        public double KerosineEuroPerMWh { get; set; }
        
        /// <summary>
        /// Gets or sets the co2 euro per ton.
        /// </summary>
        public double Co2EuroPerTon { get; set; }
        
        /// <summary>
        /// Gets or sets the wind percentage.
        /// </summary>
        public double WindPercentage { get; set; }
    }
    
    /// <summary>
    /// A representation of the powerplant.
    /// </summary>
    public class Powerplant
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PowerplantType Type { get; set; }
        
        /// <summary>
        /// Gets or sets the efficiency.
        /// </summary>
        public double Efficiency { get; set; }
        
        /// <summary>
        /// Gets or sets the Pmin.
        /// </summary>
        public int Pmin { get; set; }
        
        /// <summary>
        /// Gets or sets the Pmax.
        /// </summary>
        public int Pmax { get; set; }
    }
    
    /// <summary>
    /// An enumeration of the powerplant type.
    /// </summary>
    public enum PowerplantType
    {
        Windturbine,
        Gasfired,
        Turbojet
    }
}