using Drinks_Machine_Program.Entities.Coins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks_Machine_Program.Entities.Stock
{
    public class CoinStock
    {
        private List<Coin> Coins;

        public CoinStock()
        {
            Coins = new List<Coin>
            {
                new Quarter
                {
                    ID = 1, Name = "Quarter", Amount = 25, Value = 25
                },
                new Dime
                {
                    ID = 2, Name = "Dime", Amount = 5, Value = 10
                },
                new Nickel
                {
                    ID = 3, Name = "Nickel", Amount = 10, Value = 5
                },
                new Penny
                {
                    ID = 4, Name = "Penny", Amount = 100, Value = 1
                }
                
                
                
            };
        }

        public List<Coin> GetCoins()
        {
            return Coins;
        }

        public void SetCoins(List<Coin> Coins)
        {
            this.Coins = new List<Coin>(Coins); ;
        }
    }
}
