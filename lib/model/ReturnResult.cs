using System.Collections.Generic;

namespace ticketarena.lib.model 
{
    public class ReturnResult
    {
        public ReturnResultTypes ResultType { get; set; }
        public IEnumerable<Coin> Coins { get; set; }

    }

}