using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks_Machine_Program.Entities.Coins
{
    public abstract class Coin
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public int Value { get; set; }

        public enum Coins
        {
            Quarter,
            Dime,
            Nickel,
            Penny
        }
    }
}
