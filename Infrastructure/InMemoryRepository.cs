using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingPlaces.Core;
using TradingPlaces.Core.Exceptions;

namespace Infrastructure
{
    public class InMemoryRepository : IRepository
    {
        private readonly Dictionary<string, StrategyDetailsDto> mockDataBase = new Dictionary<string, StrategyDetailsDto>(); 


        public Task<string> CreateStrategy(StrategyDetailsDto details)
        {
            mockDataBase.Add(details.Ticker, details);

            return Task.FromResult(details.Ticker);
        }

        public Task DeleteStrategy(string ticker)
        {
            mockDataBase.Remove(ticker);

            return Task.CompletedTask;
        }

        public Task<bool> StrategyExistFlag(string ticker)
        {
            bool strategyExist = mockDataBase.ContainsKey(ticker);

            return Task.FromResult(strategyExist);
        }

        public async Task UpdateStrategyExecutedFlagAndAmount(string ticker, decimal amount)
        {
            if (!(await StrategyExistFlag(ticker)))
            {
                throw NotFoundException.ExpectedExistingStrategy(ticker);                
            }       

            mockDataBase[ticker].Executed = true;
            mockDataBase[ticker].Amount = amount;
        }

        public Task<List<StrategyDetailsDto>> GetAllUnExecutedStrategies()
        {
            List<StrategyDetailsDto> allActiveStrategies = mockDataBase.Values.Where(s => s.Executed != true).ToList();

            return Task.FromResult(allActiveStrategies);
        }
    }
}
