using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradingPlaces.Core
{
    public interface IRepository
    {
        Task<string> CreateStrategy(StrategyDetailsDto details);
        Task DeleteStrategy(string ticker);
        Task<bool> StrategyExistFlag(string ticker);
        Task UpdateStrategyExecutedFlag(string ticker);
        Task<List<StrategyDetailsDto>> GetAllUnExecutedStrategies();
    }
}
