using System.Collections.Generic;
using ticketarena.lib.model;

namespace ticketarena.lib.services
{
    public interface IVendService
    {
        IEnumerable<Product> ListProducts();

        VendResult Purchase(int productId);

        void AddCoin(Coin coin);

        IEnumerable<Coin> Coins {get;}

        ReturnResult Return();

        decimal GetCurrentValue();

        IEnumerable<CoinTypes> ListCoinsAccepted();
    }
}
