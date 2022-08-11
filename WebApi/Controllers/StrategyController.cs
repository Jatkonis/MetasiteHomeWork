using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reutberg;
using Swashbuckle.AspNetCore.Annotations;
using TradingPlaces.Core;
using TradingPlaces.WebApi.Contracts;
using TradingPlaces.WebApi.Services;

namespace TradingPlaces.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class StrategyController : ControllerBase
    {
        private readonly IHostedServiceAccessor<IStrategyManagementService> _strategyManagementService;
        private readonly IRepository _repository;
        private readonly IReutbergService _reutbergService;
        private readonly ILogger<StrategyController> _logger;

        public StrategyController(
            IHostedServiceAccessor<IStrategyManagementService> strategyManagementService,
            IRepository repository,
            IReutbergService reutbergService,
            ILogger<StrategyController> logger)
        {
            _strategyManagementService = strategyManagementService;
            _repository = repository;
            _reutbergService = reutbergService;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(nameof(RegisterStrategy))]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(string))]
        public async Task<IActionResult> RegisterStrategy(StrategyDetails strategyDetails)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();                
            }

            if (await _repository.StrategyExistFlag(strategyDetails.Ticker))
            {
                return BadRequest();
                // TODO what when such strategy exist? Should we override it?
            }

            decimal sharPrice = _reutbergService.GetQuote(strategyDetails.Ticker);    

            StrategyDetailsDto strategy = new StrategyDetailsDto()
            {                
                Ticker = strategyDetails.Ticker,
                Instruction = strategyDetails.Instruction,
                PriceMovement = strategyDetails.PriceMovement,
                Quantity = strategyDetails.Quantity,
                SharePrice = sharPrice,
                Executed = false,
                CreatedAt = DateTime.UtcNow
            };

            string result = await _repository.CreateStrategy(strategy);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(nameof(UnregisterStrategy))]
        [SwaggerResponse(StatusCodes.Status200OK, "OK")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        public async Task<IActionResult> UnregisterStrategy([FromRoute] string id)
        {
            if (!(await _repository.StrategyExistFlag(id)))
            {
                return NotFound();
                // TODO is it valid for trading purposes? What if user added wrong ID?
            }

            var result = _repository.DeleteStrategy(id);

            return Ok(result);
        }
    }
}
