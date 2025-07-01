using Asp.Versioning;
using dotNET8.Data;
using dotNET8.Dtos.Stock;
using dotNET8.Interfaces;
using dotNET8.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace dotNET8.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/stocks")]
    [ApiController]
    public class StockController(IStockRepository stockRepo) : ControllerBase
    {
        private readonly IStockRepository _stockRepo = stockRepo;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Stocks = await _stockRepo.GetAllAsync();
            var StocksDto = Stocks.Select(s => s.ToStockDto());
            return Ok(StocksDto);
        }

        [HttpGet]
        [Route("{id}")]
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
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateDto)
        {
            var StockModel = await _stockRepo.UpdateAsync(id, UpdateDto);

            return Ok(StockModel?.ToStockDto());

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _stockRepo.DeleteAsync(id);
            return NoContent();
        }

    }
}
