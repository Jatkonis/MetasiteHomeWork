using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradingPlaces.Core
{
    public interface IRepository
    {
        Task<string> CreateStrategy(StrategyDetailsDto details);
        Task DeleteStrategy(string ticker);
        Task<bool> StrategyExistFlag(string ticker);
        Task UpdateStrategyExecutedFlagAndAmount(string ticker, decimal amount);
        Task<List<StrategyDetailsDto>> GetAllUnExecutedStrategies();
    }
}
