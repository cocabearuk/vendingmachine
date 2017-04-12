using System;
using System.Linq;
using System.Collections.Generic;

using ticketarena.lib.services;
using ticketarena.lib.model;

namespace ticketarena.app
{
    public class VendingMachine
    {
        private IVendService _vendService = new VendService();
        private ICoinService _coinService = new CoinService();

        public void Run()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Welcome to the Vending Machine!");
            Console.WriteLine("================================");
            Console.WriteLine("");

            SetMessage();
            
        }

        public void SetMessage()
        {
            
            Console.WriteLine("---------------------------");
            Console.WriteLine("PRODUCTS");
            Console.WriteLine("---------------------------");
            foreach (var p in _vendService.ListProducts())
            {
                Console.WriteLine($"[{p.Code}] {p.Name} - ${p.Price}");
            }
            
            Console.WriteLine("");
            Console.WriteLine(">> INSERT COIN");
            Console.WriteLine($">> AMOUNT: ${_vendService.GetCurrentValue()}");
            Console.WriteLine("");

            var acceptedCoins = _vendService.ListCoinsAccepted();
            var coinNum = 0;
            
            foreach (var ct in acceptedCoins)
            {
                coinNum++;
                Console.WriteLine($"[{coinNum}] {ct.ToString().Replace("_","")} ");
            }

            Console.WriteLine("[R] Return Coins");
            Console.WriteLine("");
            
            Evaluate();
        }

        public void Evaluate()
        {
            
            var key = Console.ReadLine();

            if (key.StartsWith("A"))
            {
                var productId = Convert.ToInt32(key.Replace("A",""));
                var result = _vendService.Purchase(productId);
                var product = _vendService.ListProducts().SingleOrDefault(o => o.Id == productId);

                switch (result.ResultType)
                {
                    case VendResultTypes.Dispensed:
                    {
                        Console.WriteLine(">> DISPENSED. THANK YOU");
                        Console.WriteLine($">> CHANGE: ${result.Change}");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        break;
                    }
                    case VendResultTypes.Insufficient_Funds:
                    {
                        Console.WriteLine($">> PRICE ${product.Price}");
                        Console.WriteLine($">> AMOUNT: ${_vendService.GetCurrentValue()}");                        
                        break;
                    }
                    case VendResultTypes.No_Coins:
                    {
                        Console.WriteLine(">> INSERT COIN");
                        break;
                    }
                }
            }
            else if (key == "R")
            {
                var value = _vendService.GetCurrentValue();
                var result = _vendService.Return();

                switch (result.ResultType)
                {
                    case ReturnResultTypes.Returned:
                    {
                        Console.WriteLine($">> CHANGE: ${value}");
                        break;
                    }
                    case ReturnResultTypes.No_Coins:
                    {
                        Console.WriteLine($">> INSERT COIN");
                        break;
                    }
                }                
            }
            else
            {
                //Is a coin
                var index = Convert.ToInt32(key);
                var coinType = _vendService.ListCoinsAccepted().ToList()[index-1];

                var coin = _coinService.GetCoinByType(coinType);
                _vendService.AddCoin(coin);
                
            }

            SetMessage();
        }
    }
}