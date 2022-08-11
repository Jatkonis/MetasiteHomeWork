using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using TradingPlaces.Resources;
using TradingPlaces.WebApi.Attributes;

namespace TradingPlaces.WebApi.Contracts
{
    public class StrategyDetails
    {
        [TickerAttribute]        
        public string Ticker { get; set; }

        [Required]
        public BuySell Instruction { get; set; }

        [Required]
        public decimal PriceMovement { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}