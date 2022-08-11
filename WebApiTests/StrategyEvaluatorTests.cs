using System;
using TradingPlaces.Core;
using TradingPlaces.Resources;
using TradingPlaces.WebApi.Contracts;
using TradingPlaces.WebApi.Helpers;
using Xunit;

namespace Tests
{
    public class StrategyEvaluatorTests
    {
        [Theory]
        [InlineData(95, 100, 3, StrategyAction.Nothing)]
        [InlineData(95, 100, -3, StrategyAction.Buy)]
        [InlineData(95, 100, -6, StrategyAction.Nothing)]
        [InlineData(95, 100, -8, StrategyAction.Nothing)]
        [InlineData(103, 100, 2, StrategyAction.Buy)]
        [InlineData(101, 100, 2, StrategyAction.Nothing)]
        [InlineData(101, 100, -2, StrategyAction.Nothing)]
        [InlineData(105, 100, -2, StrategyAction.Nothing)]
        [InlineData(105, 100, 6, StrategyAction.Nothing)]
        [InlineData(105, 100, 4, StrategyAction.Buy)]
        public void When_StrategyCreatedStrategyEvaluator_ShouldReturnCorrectDesicion(int currentPrice, int strategyPrice, int priceMovement, StrategyAction expected)
        {
            //Arrage
            StrategyDetailsDto strategy = BuildStrategy(priceMovement, strategyPrice);            
            StrategyEvaluator sut = new StrategyEvaluator();

            //Act
            var result = sut.Evaluate(strategy, currentPrice);

            //Assert
            Assert.Equal(expected, result);
        }        

        private StrategyDetailsDto BuildStrategy(int priceMovement, int sharePrice)
        {
            return new StrategyDetailsDto()
            {
                Ticker = "SCOO",
                Instruction = BuySell.Buy,
                PriceMovement = (decimal)priceMovement,
                SharePrice = (decimal)sharePrice,
                Executed = false,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
