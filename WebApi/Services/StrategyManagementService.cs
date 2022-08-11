using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TradingPlaces.Resources;
using Reutberg;
using System.Collections.Generic;
using TradingPlaces.Core;
using TradingPlaces.WebApi.Contracts;
using TradingPlaces.WebApi.Helpers;

namespace TradingPlaces.WebApi.Services
{
    public class StrategyManagementService : TradingPlacesBackgroundServiceBase, IStrategyManagementService
    {
        private const int TickFrequencyMilliseconds = 1000;
        private IReutbergService _reutbergService;
        private readonly IRepository _repository;
        private readonly IStrategyEvaluator _strategyEvaluator;

        public StrategyManagementService(
            IReutbergService service, 
            ILogger<StrategyManagementService> logger, 
            IRepository repository,
            IStrategyEvaluator strategyEvaluator
            )
            : base(TimeSpan.FromMilliseconds(TickFrequencyMilliseconds), logger)
        {
            _reutbergService = service;
            _repository = repository;
            _strategyEvaluator = strategyEvaluator;
        }

        protected override async Task CheckStrategies()
        {
            List<StrategyDetailsDto> allStrategies = await _repository.GetAllUnExecutedStrategies();

            foreach (var strategy in allStrategies)
            {
                decimal tickerCurrentPrice = _reutbergService.GetQuote(strategy.Ticker);                               

                StrategyAction action = _strategyEvaluator.Evaluate(strategy, tickerCurrentPrice);

                if (action == StrategyAction.Buy)
                {
                    var amount = _reutbergService.Buy(strategy.Ticker, strategy.Quantity);
                    await _repository.UpdateStrategyExecutedFlagAndAmount(strategy.Ticker, (decimal)amount);
                }
                else if (action == StrategyAction.Sell)
                {
                    var amount =_reutbergService.Sell(strategy.Ticker, strategy.Quantity);
                    await _repository.UpdateStrategyExecutedFlagAndAmount(strategy.Ticker, (decimal)amount);
                }
                else 
                {
                    //TODO log?
                }
            }            
        }
    }
}
