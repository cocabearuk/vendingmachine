using System.Collections.Generic;
using ticketarena.lib.model;

namespace ticketarena.lib.services
{
    public interface ICoinService
    {
        
        IEnumerable<Coin> ListCoins();
        Coin GetCoinByType(CoinTypes coinType);
    }
}
