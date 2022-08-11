using TradingPlaces.Core;
using TradingPlaces.WebApi.Contracts;

namespace TradingPlaces.WebApi.Helpers
{
    public interface IStrategyEvaluator
    {
        StrategyAction Evaluate(StrategyDetailsDto strategy, decimal currentSharPrice);
    }
}
