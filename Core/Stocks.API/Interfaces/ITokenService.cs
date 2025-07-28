using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stocks.API.Models;

namespace Stocks.API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}