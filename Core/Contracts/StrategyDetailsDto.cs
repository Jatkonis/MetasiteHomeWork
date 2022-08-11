using System;
using TradingPlaces.Resources;

namespace TradingPlaces.Core
{
    public class StrategyDetailsDto
    {        
        public string Ticker { get; set; }
        public BuySell Instruction { get; set; }
        public decimal PriceMovement { get; set; }
        public int Quantity { get; set; }
        public decimal SharePrice { get; set; }
        public bool Executed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
