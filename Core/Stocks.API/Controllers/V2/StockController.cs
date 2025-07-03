using Asp.Versioning;
using Stocks.API.Data;
using Stocks.API.Dtos.Stock;
using Stocks.API.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Stocks.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/stock")]
    [ApiController]
    public class StockController(ApplicationDBContext context) : ControllerBase
    {
        private readonly ApplicationDBContext _context = context;

        [HttpGet]
        public IActionResult GetAll()
        {
            var stocks = _context.Stocks.Select(s => s.ToStockDto()).ToList();
            
            // V2 adds a message to demonstrate the difference
            return Ok(new { 
                Message = "This is version 2.0 of the API", 
                Data = stocks 
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
            
            if (stock is null) 
                return NotFound();
                
            // V2 adds a message to demonstrate the difference
            return Ok(new { 
                Message = "This is version 2.0 of the API", 
                Data = stock.ToStockDto() 
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto StockDto)
        {
            var StockModel = StockDto.ToStock();
            _context.Stocks.Add(StockModel);
            _context.SaveChanges();
            
            return CreatedAtAction(nameof(GetById), new { id = StockModel.Id, version = "2.0" }, 
                new { 
                    Message = "This is version 2.0 of the API", 
                    Data = StockModel.ToStockDto() 
                });
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateDto)
        {
            var StockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);

            if (StockModel is null) return NotFound();

            UpdateDto.MapStockDtoToStockModel(StockModel);

            _context.SaveChanges();

            return Ok(new { 
                Message = "This is version 2.0 of the API", 
                Data = StockModel.ToStockDto() 
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var StockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);

            if (StockModel is null) return NotFound();

            _context.Remove(StockModel);

            _context.SaveChanges();

            return NoContent();
        }
    }
}
