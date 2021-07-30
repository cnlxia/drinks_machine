using Drinks_Machine_Program.Entities.Coins;
using Drinks_Machine_Program.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks_Machine_Program.Models
{
    public abstract class VendingMachineModel
    {
        public List<Item> ItemsStock { get; set; }

        public List<Coin> CoinsStock { get; set; }


    }
}
