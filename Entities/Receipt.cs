using Drinks_Machine_Program.Entities.Coins;
using Drinks_Machine_Program.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks_Machine_Program.Entities
{
    public class Receipt
    {
        public int TotalPaid { get; set; }

        public int TotalCost { get; set; }

        public List<Item> ItemsBought { get; set; }

        public Dictionary<Coin.Coins , int> Change { get; set; }

        public bool IsEnoughChange { get; set; }

        public bool IsEnoughItems { get; set; }

        public Receipt()
        {
            ItemsBought = new List<Item>();
            Change = new Dictionary<Coin.Coins, int>();
        }
    }
}
