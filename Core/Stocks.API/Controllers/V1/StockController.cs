using Asp.Versioning;
using Stocks.API.Data;
using Stocks.API.Dtos.Stock;
using Stocks.API.Helpers;
using Stocks.API.Interfaces;
using Stocks.API.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Stocks.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/stocks")]
    [ApiController]
    public class StockController(IStockRepository stockRepo) : ControllerBase
    {
        private readonly IStockRepository _stockRepo = stockRepo;


        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var Stocks = await _stockRepo.GetAllAsync(query);
            var StocksDto = Stocks.Select(s => s.ToStockDto());
            return Ok(StocksDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepo.GetByIdAsync(id);

            return stock is null ? NotFound() : Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto StockDto)
        {
            var stockModel = StockDto.ToStock();
            await _stockRepo.CreateAsync(stockModel);

            return CreatedAtAction("GetbyId", new { id = stockModel.Id }, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateDto)
        {
            var StockModel = await _stockRepo.UpdateAsync(id, UpdateDto);

            return Ok(StockModel?.ToStockDto());

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _stockRepo.DeleteAsync(id);
            return NoContent();
        }

    }
}
