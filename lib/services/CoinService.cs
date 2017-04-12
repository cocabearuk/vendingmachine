using System.Linq;
using System.Collections.Generic;
using ticketarena.lib.model;

namespace ticketarena.lib.services
{
    public sealed class CoinService : ICoinService
    {

        public IEnumerable<Coin> ListCoins()
        {
            return new Coin[] {
                new Coin { Weight = 1.5m, Diameter = 2.0m, Value = 5.00m, CoinType = CoinTypes.FiveDollars },
                new Coin { Weight = 1.4m, Diameter = 1.3m, Value = 1.00m, CoinType = CoinTypes.OneDollar },
                new Coin { Weight = 1.3m, Diameter = 1.2m, Value = 0.5m, CoinType = CoinTypes.FiftyCents },
                new Coin { Weight = 0.8m, Diameter = 1.1m, Value = 0.1m, CoinType = CoinTypes.TenCents },
                new Coin { Weight = 0.5m, Diameter = 1.0m, Value = 0.05m, CoinType = CoinTypes.FiveCents }
            };
        }

        public Coin GetCoinByType(CoinTypes type)
        {
            return ListCoins().SingleOrDefault(o => o.CoinType == type);
        }

    }
}
