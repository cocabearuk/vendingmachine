using System.Linq;
using Xunit;

using ticketarena.lib.services;
using ticketarena.lib.model;

namespace ticketarena.tests
{
    public class CoinTests
    {
        private readonly ICoinService _coinService;

        public CoinTests()
        {
            _coinService = new CoinService();
        }

        [Fact]
        public void Coins_GetCoinValue_ShouldReturnOneDollar()
        {
            var coinType = CoinTypes.OneDollar;
            var coin = _coinService.GetCoinByType(coinType);
            Assert.Equal(coin.Value, 1.00m);
            Assert.Equal(coin.CoinType, coinType);
        }


        [Fact]
        public void Coins_GetCoinByType_ShouldReturnMatchingCoin()
        {
            var coinType = CoinTypes.OneDollar;
            var coin = _coinService.GetCoinByType(coinType);
            Assert.Equal(coinType, coin.CoinType);
        }

    }
}
