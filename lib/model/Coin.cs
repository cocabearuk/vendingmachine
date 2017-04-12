
namespace ticketarena.lib.model 
{
    public class Coin
    {
        public decimal Weight { get; set; }
        public decimal Diameter { get; set; }
        public decimal Thickness { get;set; }
        public CoinTypes CoinType {get;set;}
        public decimal Value {get;set;}
    }

}