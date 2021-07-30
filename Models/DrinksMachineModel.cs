using Drinks_Machine_Program.Entities.Coins;
using Drinks_Machine_Program.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks_Machine_Program.Models
{
    public class DrinksMachineModel : VendingMachineModel
    {
        public DrinksMachineModel()
        {
            ItemsStock = new List<Item>();
            CoinsStock = new List<Coin>();
        }
    }
}
