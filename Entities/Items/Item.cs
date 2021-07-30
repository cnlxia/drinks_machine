using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks_Machine_Program.Entities.Items
{
    public abstract class Item
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int Cost { get; set; }
    }
}
