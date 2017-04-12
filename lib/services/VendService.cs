using System;
using System.Linq;
using System.Collections.Generic;
using ticketarena.lib.model;

namespace ticketarena.lib.services
{
    public class VendService : IVendService
    {
        private List<Coin> _coins;
        private ICoinService _coinService;

        public VendService()
        {
            _coins = new List<Coin>();
            _coinService = new CoinService();
        }

        public IEnumerable<Coin> Coins {
            get {
                return _coins;
            }
        }

        public IEnumerable<CoinTypes> ListCoinsAccepted()
        {
            return new CoinTypes[] { CoinTypes.FiveCents, CoinTypes.TenCents, CoinTypes.FiftyCents, CoinTypes.OneDollar };
        }

        public ReturnResult Return()
        {
            var result = new ReturnResult { ResultType = ReturnResultTypes.Unknown, Coins = new Coin[]{}};
            if (_coins.Count == 0)
            {
                result.ResultType = ReturnResultTypes.No_Coins;
            }
            else
            {  
                result.ResultType = ReturnResultTypes.Returned;
                result.Coins = _coins.ToArray();
                _coins.Clear();
            }
            return result;
        }

        public decimal GetCurrentValue()
        {
            var all = (from c in _coinService.ListCoins()
            join ct in _coins on new { c .Weight, c.Diameter } equals new { ct.Weight, ct.Diameter }
            select c.Value).Sum();
            return all;            
        }

        public IEnumerable<Product> ListProducts()
        {
            return new Product[] {
                new Product { Id = 1, Name = "cola", Price = 1.00m, Code="A1" },
                new Product { Id = 2, Name = "chips", Price = 0.5m, Code="A2" },
                new Product { Id = 3, Name = "candy", Price = 0.65m, Code="A3" }
            };       
        }

        public void AddCoin(Coin coin)
        {
            if (coin.CoinType != CoinTypes.FiveDollars) {
                _coins.Add(coin);
            }
            else {
                throw new ArgumentException("This coin type is not accepted.");
            }
        }

        public VendResult Purchase(int productId)
        {
            var result = new VendResult { ResultType = VendResultTypes.Unknown, Change = 0};
            if (_coins.Count == 0) {
                result.ResultType = VendResultTypes.No_Coins; 
            }
            else {
            
                var product = ListProducts().SingleOrDefault(o => o.Id == productId);
                var coinValue = GetCurrentValue();

                result.ResultType = VendResultTypes.Unknown;
                result.Change = 0;

                if (product != null) {

                    if (product.Price <= coinValue) {

                        result.ResultType = VendResultTypes.Dispensed;
                        result.Change = coinValue - product.Price;
                        
                        _coins.Clear();
                    }
                    else {
                        result.ResultType = VendResultTypes.Insufficient_Funds;
                    }

                }
            }

            return result;
        }
        
    }
}
