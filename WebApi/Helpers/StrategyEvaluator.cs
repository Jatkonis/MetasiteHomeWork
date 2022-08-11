using System;
using TradingPlaces.Core;
using TradingPlaces.Resources;
using TradingPlaces.WebApi.Contracts;

namespace TradingPlaces.WebApi.Helpers
{
    public class StrategyEvaluator : IStrategyEvaluator
    {
        public StrategyAction Evaluate(StrategyDetailsDto strategy, decimal currentSharePrice)
        {
            bool sharePriceSameOrIncreased = currentSharePrice >= strategy.SharePrice;
            bool strategyExpectsIncrease = strategy.PriceMovement >= 0;

            decimal differenceInPrice = CalculateDifference(sharePriceSameOrIncreased, strategy, currentSharePrice);          

            if(strategy.PriceMovement < 0 && sharePriceSameOrIncreased == true)
            {
                return StrategyAction.Nothing;
            }

            if (strategyExpectsIncrease && sharePriceSameOrIncreased == false)
            {
                return StrategyAction.Nothing;
            }

            if (Math.Abs(strategy.PriceMovement) < differenceInPrice)
            {
                return strategy.Instruction == BuySell.Sell ? StrategyAction.Sell : StrategyAction.Buy;
            }      

            return StrategyAction.Nothing;
        }

        private decimal CalculateDifference(bool sharePriceSameOrIncreased, StrategyDetailsDto strategy, decimal currentSharePrice)
        {
            if (sharePriceSameOrIncreased)
            {
                return (currentSharePrice - strategy.SharePrice) / strategy.SharePrice * 100;
            }
            else
            {
                return (strategy.SharePrice - currentSharePrice) / strategy.SharePrice * 100;
            }
        }
    }
}
