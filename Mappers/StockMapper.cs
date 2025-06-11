using dotNET8.Dtos.Stock;
using dotNET8.Models;

namespace dotNET8.Mappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock StockModel)
        {
            return new StockDto
            {
                Id = StockModel.Id,
                Symbol = StockModel.Symbol,
                CompanyName = StockModel.CompanyName,
                Purchase = StockModel.Purchase,
                LastDiv = StockModel.LastDiv,
                Industry = StockModel.Industry,
                MarketCap = StockModel.MarketCap

            };
        }

        public static Stock ToStock(this CreateStockRequestDto StockDto)
        {
            return new Stock
            {
                Symbol = StockDto.Symbol,
                CompanyName = StockDto.CompanyName,
                Purchase = StockDto.Purchase,
                LastDiv = StockDto.LastDiv,
                Industry = StockDto.Industry,
                MarketCap = StockDto.MarketCap

            };
        }

        public static void MapStockDtoToStockModel(this UpdateStockRequestDto src, Stock dest)
        {
            dest.Symbol      = src.Symbol;
            dest.CompanyName = src.CompanyName;
            dest.Purchase    = src.Purchase;
            dest.LastDiv     = src.LastDiv;
            dest.Industry    = src.Industry;
            dest.MarketCap   = src.MarketCap;
        }

    }
}