using System;
using System.Linq;
using Xunit;

using ticketarena.lib.services;
using ticketarena.lib.model;

namespace ticketarena.tests
{
    public class VendTests
    {
        private readonly IVendService _vendService;
        private readonly ICoinService _coinService;

        public VendTests()
        {
            _vendService = new VendService();
            _coinService = new CoinService();
        }

        

        [Fact]
        public void Vend_ListVend_ShouldReturnThreeProducts()
        {
            var products = _vendService.ListProducts();
            Assert.True(products.Count() == 3);
        }

        [Fact]
        public void Vend_PurchaseProduct_ShouldReturnThankYouNoChange()
        {
            var colaProductId = 1;
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.OneDollar));
            var result = _vendService.Purchase(colaProductId);

            Assert.Equal(result.ResultType, VendResultTypes.Dispensed);
            Assert.Equal(result.Change, 0);
            Assert.Equal(_vendService.Coins.Count(),0);
        }

        [Fact]
        public void Vend_PurchaseProduct_DuplicateCoins_ShouldReturnThankYouNoChange()
        {
            var colaProductId = 1;
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiftyCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiftyCents));
            var result = _vendService.Purchase(colaProductId);

            Assert.Equal(result.ResultType, VendResultTypes.Dispensed);
            Assert.Equal(result.Change, 0);
            Assert.Equal(_vendService.Coins.Count(),0);
        }

        [Fact]
        public void Vend_PurchaseProduct_ShouldReturnThankYouWithChange()
        {
            var chipsProductId = 2;
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.OneDollar));
            var result = _vendService.Purchase(chipsProductId);

            Assert.Equal(result.ResultType, VendResultTypes.Dispensed);
            Assert.Equal(0.5m, result.Change);
            Assert.Equal(0, _vendService.Coins.Count());
        }

        [Fact]
        public void Vend_PurchaseProduct_ShouldReturnNoCoins()
        {
            var colaProductId = 1;
            var result = _vendService.Purchase(colaProductId);

            Assert.Equal(result.ResultType, VendResultTypes.No_Coins);
            Assert.Equal(0, result.Change);
            Assert.Equal(0, _vendService.Coins.Count());
        }

        [Fact]
        public void Vend_PurchaseProduct_ShouldReturnInsufficientFunds()
        {
            var candyProductId = 3;
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            var result = _vendService.Purchase(candyProductId);

            Assert.Equal(result.ResultType, VendResultTypes.Insufficient_Funds);
            Assert.Equal(0, result.Change);
            Assert.Equal(1, _vendService.Coins.Count());
        }

        [Fact]
        public void Vend_AddCoin_ShouldReturnCurrentTotal()
        {
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.OneDollar));
            var curValue = _vendService.GetCurrentValue();
            Assert.Equal(curValue, 1.05m);
            Assert.Equal(_vendService.Coins.Count(), 2);
        }

        [Fact]
        public void Vend_AddCoin_ShouldReturnUnacceptable()
        {
            Exception ex = Assert.Throws<ArgumentException>(() => _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveDollars)));
            
           Assert.Equal(ex.Message, "This coin type is not accepted.");
        }

        [Fact]
        public void Product_ReturnCoins_ShouldReturnCoinsEquallingTotal()
        {
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));

            var curValue = _vendService.GetCurrentValue();
            var result = _vendService.Return();
            Assert.Equal(result.ResultType, ReturnResultTypes.Returned);
            Assert.Equal(curValue, 0.5m);
            Assert.Equal(_vendService.Coins.Count(), 0);
        }

        [Fact]
        public void Product_ReturnCoins_ShouldReturnNoCoinsNoValue()
        {
            var curValue = _vendService.GetCurrentValue();
            var result = _vendService.Return();
            Assert.Equal(result.ResultType, ReturnResultTypes.No_Coins);
            Assert.Equal(curValue, 0m);
            Assert.Equal(_vendService.Coins.Count(), 0);
        }

        [Fact]
        public void Vend_GetCurrentValue_ShouldReturnOneDollarSixtyFive()
        {
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.OneDollar));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiveCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.FiftyCents));
            _vendService.AddCoin(_coinService.GetCoinByType(CoinTypes.TenCents));
            
            var value = _vendService.GetCurrentValue();
            Assert.True(value == 1.65m);
        }

        [Fact]
        public void Coins_ListAcceptedCoins_ShouldReturnAllCoinTypes()
        {
            var coinTypes = _vendService.ListCoinsAccepted();
            Assert.False(coinTypes.Contains(CoinTypes.FiveDollars));
        }

    }
}
